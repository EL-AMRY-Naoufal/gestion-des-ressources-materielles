using GestionDesRessourcesMaterielles.Data;
using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDesRessourcesMaterielles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FournisseurController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;

        public FournisseurController(ApplicationDbContext applicationDbContext)
        {
            _authContext = applicationDbContext;
        }

        [HttpPost("submitOffer")]
        public async Task<IActionResult> SubmitOffer(int appelOffreId, int fournisseurId, double montant)
        {
            try
            {
                // Check if the AppelOffre exists
                var appelOffre = await _authContext.AppelOffres.FindAsync(appelOffreId);
                if (appelOffre == null)
                {
                    return NotFound("AppelOffre not found");
                }

                // Check if the Fournisseur exists
                var fournisseur = await _authContext.Fournisseurs.FindAsync(fournisseurId);
                if (fournisseur == null)
                {
                    return NotFound("Fournisseur not found");
                }

                // Create the offer
                var offreFournisseur = new OffreFournisseur
                {
                    AppelOffreId = appelOffre,
                    FournisseurId = fournisseur,
                    Montant = montant,
                    DateSoumission = DateTime.Now
                };

                // Add the offer to the context and save changes
                _authContext.offreFournisseurs.Add(offreFournisseur);
                await _authContext.SaveChangesAsync();

                return Ok("Offer submitted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("appelOffres")]
        public async Task<IActionResult> GetAppelOffres()
        {
            try
            {
                // Retrieve all available AppelOffres
                var appelOffres = await _authContext.AppelOffres
                    .Include(a => a.Departement)
                    .Include(a => a.Besoins)
                    .ThenInclude(b => b.RessourceCatalogteId)
                    .ToListAsync();

                return Ok(appelOffres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("changeMontant")]
        public async Task<IActionResult> SubmitOffer(int offreFournisseurId, double montant)
        {
            try
            {
                // Check if the existing OffreFournisseur exists
                var existingOffer = await _authContext.offreFournisseurs.FindAsync(offreFournisseurId);
                if (existingOffer == null)
                {
                    return NotFound("OffreFournisseur not found");
                }

                // Check if the existing offer has been treated
                if (existingOffer.IsAccepted != null)
                {
                    return BadRequest("Cannot update treated offer");
                }

                // Update the montant of the existing offer
                existingOffer.Montant = montant;
                existingOffer.DateSoumission = DateTime.Now;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok("Offer updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("offresFournisseur")]
        public async Task<IActionResult> GetNonTreatedOffresFournisseur(int fournisseurId)
        {
            try
            {
                // Retrieve non-treated offers for the specified Fournisseur
                var offres = await _authContext.offreFournisseurs
                    .Where(o => o.FournisseurId.UserId == fournisseurId && o.IsAccepted == null)
                    .Include(o => o.FournisseurId)
                    .Include(o => o.AppelOffreId)
                    .ToListAsync();

                return Ok(offres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("acceptedOffresFournisseur")]
        public async Task<IActionResult> GetAcceptedOffresFournisseur(int fournisseurId)
        {
            try
            {
                var offres = await _authContext.offreFournisseurs
                                    .Where(o => o.FournisseurId.UserId == fournisseurId && o.IsAccepted == true && o.IsApproved == false)
                                    .Include(o => o.FournisseurId)
                                    .Include(o => o.AppelOffreId)
                                        .ThenInclude(a => a.Departement)
                                    .Include(o => o.AppelOffreId)
                                        .ThenInclude(a => a.Besoins)
                                            .ThenInclude(b => b.RessourceCatalogteId)
                                    .Include(o => o.AppelOffreId)
                                        .ThenInclude(a => a.Besoins)
                                            .ThenInclude(b => b.PersonneDepartementId) 
                                    .ToListAsync();

                return Ok(offres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("createRessource")]
        public async Task<IActionResult> CreateRessource(int fournisseurID, int appelOffreID, DateTime deliveryDate)
        {
            try
            {
                var fournisseur = await _authContext.Fournisseurs.FindAsync(fournisseurID);
                if (fournisseur == null)
                {
                    return NotFound("Fournisseur not found");
                }

                var besoins = await _authContext.Besoins
                    .Include(b => b.RessourceCatalogteId)
                    .Include(b => b.PersonneDepartementId)
                    .ThenInclude(pd => pd.Departement) // Include the Departement from PersonneDepartement
                    .Include(b => b.AppelOffre) // Include the AppelOffre
                    .Where(b => b.AppelOffre.AppelOffreID == appelOffreID)
                    .ToListAsync();

                if (besoins == null || !besoins.Any())
                {
                    return NotFound("No Besoins found for the specified AppelOffre");
                }

                // Loop through each Besoin and create the corresponding Ressource
                foreach (var besoin in besoins)
                {
                    // Create the corresponding Ressource based on the type of RessourceCatalog
                    if (besoin.RessourceCatalogteId is OrdinateurCatalog ordinateurCatalog)
                    {
                        // If it's an OrdinateurCatalog, create an Ordinateur
                        var ordinateur = new Ordinateur
                        {
                            Marque = ordinateurCatalog.Marque,
                            Cpu = ordinateurCatalog.Cpu,
                            Ram = ordinateurCatalog.Ram,
                            DisqueDur = ordinateurCatalog.DisqueDur,
                            Ecran = ordinateurCatalog.Ecran,
                            DateLivraison = deliveryDate,
                            FournisseurId = fournisseur,
                            PersonneDepartement = besoin.PersonneDepartementId,
                            Departement = besoin.PersonneDepartementId?.Departement
                        };

                        _authContext.Ordinateurs.Add(ordinateur);
                    }
                    else if (besoin.RessourceCatalogteId is ImprimanteCatalog imprimanteCatalog)
                    {
                        // If it's an ImprimanteCatalog, create an Imprimante
                        var imprimante = new Imprimante
                        {
                            Vitesseimpression = imprimanteCatalog.Vitesseimpression,
                            Resolution = imprimanteCatalog.Resolution,
                            Marque = imprimanteCatalog.Marque,
                            DateLivraison = deliveryDate,
                            FournisseurId = fournisseur,
                            PersonneDepartement = besoin.PersonneDepartementId,
                            Departement = besoin.PersonneDepartementId?.Departement
                        };

                        // Add the Imprimante to the context
                        _authContext.Imprimantes.Add(imprimante);
                    }
                    var offresFournisseur = await _authContext.offreFournisseurs
                                            .Where(o => o.AppelOffreId.AppelOffreID == besoin.AppelOffre.AppelOffreID)
                                            .ToListAsync();

                    if (offresFournisseur == null || !offresFournisseur.Any())
                    {
                        return NotFound("No OffreFournisseur found for the specified AppelOffre");
                    }

                    // Loop through each OffreFournisseur and set IsApproved to true
                    foreach (var offreFournisseur in offresFournisseur)
                    {
                        offreFournisseur.IsApproved = true;
                    }

                    // Save changes to the database
                    await _authContext.SaveChangesAsync();
                }

                // Save changes to the context
                await _authContext.SaveChangesAsync();

                return Ok("Ressources created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("catalogs")]
        public async Task<IActionResult> GetCatalogs()
        {
            try
            {
                var imprimantCatalogs = await _authContext.ImprimanteCatalog.ToListAsync();
                var ordinateurCatalogs = await _authContext.OrdinateurCatalog.ToListAsync();

                return Ok(new { ImprimantCatalogs = imprimantCatalogs, OrdinateurCatalogs = ordinateurCatalogs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}

