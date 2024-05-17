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
        public async Task<IActionResult> SubmitOffer([FromBody] SubmitOfferRequest request)
        {
            try
            {
                // Check if the AppelOffre exists
                var appelOffre = await _authContext.AppelOffres.FindAsync(request.AppelOffreId);
                if (appelOffre == null)
                {
                    return NotFound("AppelOffre not found");
                }

                // Check if the Fournisseur exists
                var fournisseur = await _authContext.Fournisseurs.FindAsync(request.FournisseurId);
                if (fournisseur == null)
                {
                    return NotFound("Fournisseur not found");
                }

                // Create the offer
                var offreFournisseur = new OffreFournisseur
                {
                    AppelOffreId = appelOffre,
                    FournisseurId = fournisseur,
                    Montant = request.Montant,
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
        public async Task<IActionResult> GetAppelOffres(int fournisseurId)
        {
            try
            {
                var appelOffres = await _authContext.AppelOffres
                    .Include(a => a.Departement)
                    .Include(a => a.Besoins)
                    .ThenInclude(b => b.RessourceCatalogteId)
                    .Where(a => !_authContext.offreFournisseurs.Any(o => o.AppelOffreId.AppelOffreID == a.AppelOffreID && o.FournisseurId.UserId == fournisseurId))
                    .ToListAsync();

                var result = appelOffres.Select(a => new AppelOffreDto
                {
                    AppelOffreID = a.AppelOffreID,
                    DepartementName = a.Departement?.Name,
                    OrdinateurBesoins = a.Besoins
                        .Where(b => b.RessourceCatalogteId is OrdinateurCatalog)
                        .Select(b => new OrdinateurBesoinDto
                        {
                            BesoinId = b.BesoinId,
                            Marque = (b.RessourceCatalogteId as OrdinateurCatalog)?.Marque,
                            Cpu = (b.RessourceCatalogteId as OrdinateurCatalog)?.Cpu,
                            Ram = (b.RessourceCatalogteId as OrdinateurCatalog)?.Ram,
                            DisqueDur = (b.RessourceCatalogteId as OrdinateurCatalog)?.DisqueDur,
                            Ecran = (b.RessourceCatalogteId as OrdinateurCatalog)?.Ecran,
                            NumberOfRessource = b.NumberOfRessource
                        }).ToList(),
                    ImprimanteBesoins = a.Besoins
                        .Where(b => b.RessourceCatalogteId is ImprimanteCatalog)
                        .Select(b => new ImprimanteBesoinDto
                        {
                            BesoinId = b.BesoinId,
                            Marque = (b.RessourceCatalogteId as ImprimanteCatalog)?.Marque,
                            Vitesseimpression = (b.RessourceCatalogteId as ImprimanteCatalog)?.Vitesseimpression ?? 0,
                            Resolution = (b.RessourceCatalogteId as ImprimanteCatalog)?.Resolution,
                            NumberOfRessource = b.NumberOfRessource
                        }).ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("changeMontant")]
        public async Task<IActionResult> SubmitOffer([FromBody] UpdateOfferDto updateOfferDto)
        {
            try
            {
                // Check if the existing OffreFournisseur exists
                var existingOffer = await _authContext.offreFournisseurs
                                           .FirstOrDefaultAsync(o => o.AppelOffreId.AppelOffreID == updateOfferDto.AppelOffreId);

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
                existingOffer.Montant = updateOfferDto.Montant;
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
                var appelOffres = await _authContext.AppelOffres
                    .Include(a => a.Departement)
                    .Include(a => a.Besoins)
                    .ThenInclude(b => b.RessourceCatalogteId)
                    .Where(a => _authContext.offreFournisseurs.Any(o => o.AppelOffreId.AppelOffreID == a.AppelOffreID && o.FournisseurId.UserId == fournisseurId))
                    .ToListAsync();

                var result = appelOffres.Select(a => new AppelOffreDto
                {
                    AppelOffreID = a.AppelOffreID,
                    DepartementName = a.Departement?.Name,
                    OrdinateurBesoins = a.Besoins
                        .Where(b => b.RessourceCatalogteId is OrdinateurCatalog)
                        .Select(b => new OrdinateurBesoinDto
                        {
                            BesoinId = b.BesoinId,
                            Marque = (b.RessourceCatalogteId as OrdinateurCatalog)?.Marque,
                            Cpu = (b.RessourceCatalogteId as OrdinateurCatalog)?.Cpu,
                            Ram = (b.RessourceCatalogteId as OrdinateurCatalog)?.Ram,
                            DisqueDur = (b.RessourceCatalogteId as OrdinateurCatalog)?.DisqueDur,
                            Ecran = (b.RessourceCatalogteId as OrdinateurCatalog)?.Ecran,
                            NumberOfRessource = b.NumberOfRessource
                        }).ToList(),
                    ImprimanteBesoins = a.Besoins
                        .Where(b => b.RessourceCatalogteId is ImprimanteCatalog)
                        .Select(b => new ImprimanteBesoinDto
                        {
                            BesoinId = b.BesoinId,
                            Marque = (b.RessourceCatalogteId as ImprimanteCatalog)?.Marque,
                            Vitesseimpression = (b.RessourceCatalogteId as ImprimanteCatalog)?.Vitesseimpression ?? 0,
                            Resolution = (b.RessourceCatalogteId as ImprimanteCatalog)?.Resolution,
                            NumberOfRessource = b.NumberOfRessource
                        }).ToList(),
                    IsAccepted = _authContext.offreFournisseurs
                                .Where(o => o.AppelOffreId.AppelOffreID == a.AppelOffreID && o.FournisseurId.UserId == fournisseurId)
                                .Select(o => o.IsAccepted)
                                .FirstOrDefault()
                }).ToList();

                return Ok(result);
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
                var appelOffres = await _authContext.AppelOffres
                    .Include(a => a.Departement)
                    .Include(a => a.Besoins)
                    .ThenInclude(b => b.RessourceCatalogteId)
                    .Where(a => _authContext.offreFournisseurs.Any(o => o.AppelOffreId.AppelOffreID == a.AppelOffreID && o.FournisseurId.UserId == fournisseurId && o.IsAccepted == true))
                    .ToListAsync();

                var result = appelOffres.Select(a => new AppelOffreDto
                {
                    AppelOffreID = a.AppelOffreID,
                    DepartementName = a.Departement?.Name,
                    OrdinateurBesoins = a.Besoins
                        .Where(b => b.RessourceCatalogteId is OrdinateurCatalog)
                        .Select(b => new OrdinateurBesoinDto
                        {
                            BesoinId = b.BesoinId,
                            Marque = (b.RessourceCatalogteId as OrdinateurCatalog)?.Marque,
                            Cpu = (b.RessourceCatalogteId as OrdinateurCatalog)?.Cpu,
                            Ram = (b.RessourceCatalogteId as OrdinateurCatalog)?.Ram,
                            DisqueDur = (b.RessourceCatalogteId as OrdinateurCatalog)?.DisqueDur,
                            Ecran = (b.RessourceCatalogteId as OrdinateurCatalog)?.Ecran,
                            NumberOfRessource = b.NumberOfRessource
                        }).ToList(),
                    ImprimanteBesoins = a.Besoins
                        .Where(b => b.RessourceCatalogteId is ImprimanteCatalog)
                        .Select(b => new ImprimanteBesoinDto
                        {
                            BesoinId = b.BesoinId,
                            Marque = (b.RessourceCatalogteId as ImprimanteCatalog)?.Marque,
                            Vitesseimpression = (b.RessourceCatalogteId as ImprimanteCatalog)?.Vitesseimpression ?? 0,
                            Resolution = (b.RessourceCatalogteId as ImprimanteCatalog)?.Resolution,
                            NumberOfRessource = b.NumberOfRessource
                        }).ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("createRessource")]
        public async Task<IActionResult> CreateRessource([FromBody] CreateRessourceDto createRessourceDto)
        {
            try
            {
                var fournisseur = await _authContext.Fournisseurs.FindAsync(createRessourceDto.FournisseurID);
                if (fournisseur == null)
                {
                    return NotFound("Fournisseur not found");
                }

                var besoins = await _authContext.Besoins
                    .Include(b => b.RessourceCatalogteId)
                    .Include(b => b.PersonneDepartementId)
                    .ThenInclude(pd => pd.Departement) // Include the Departement from PersonneDepartement
                    .Include(b => b.AppelOffre) // Include the AppelOffre
                    .Where(b => b.AppelOffre.AppelOffreID == createRessourceDto.AppelOffreID)
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
                            DateLivraison = createRessourceDto.DeliveryDate,
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
                            DateLivraison = createRessourceDto.DeliveryDate,
                            FournisseurId = fournisseur,
                            PersonneDepartement = besoin.PersonneDepartementId,
                            Departement = besoin.PersonneDepartementId?.Departement
                        };

                        // Add the Imprimante to the context
                        _authContext.Imprimantes.Add(imprimante);
                    }

                    // Update OffreFournisseur status
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

    public class AppelOffreDto
    {
        public int AppelOffreID { get; set; }
        public string DepartementName { get; set; }
        public List<OrdinateurBesoinDto> OrdinateurBesoins { get; set; }
        public List<ImprimanteBesoinDto> ImprimanteBesoins { get; set; }
        public bool? IsAccepted {  get; set; }
    }

    public class OrdinateurBesoinDto
    {
        public int BesoinId { get; set; }
        public string Marque { get; set; }
        public string Cpu { get; set; }
        public string Ram { get; set; }
        public string DisqueDur { get; set; }
        public string Ecran { get; set; }
        public int NumberOfRessource { get; set; }
    }

    public class ImprimanteBesoinDto
    {
        public int BesoinId { get; set; }
        public string Marque { get; set; }
        public int Vitesseimpression { get; set; }
        public string Resolution { get; set; }
        public int NumberOfRessource { get; set; }
    }

    public class SubmitOfferRequest
    {
        public int AppelOffreId { get; set; }
        public int FournisseurId { get; set; }
        public double Montant { get; set; }
    }

    public class UpdateOfferDto
    {
        public int AppelOffreId { get; set; }
        public double Montant { get; set; }
    }

    public class CreateRessourceDto
    {
        public int FournisseurID { get; set; }
        public int AppelOffreID { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}

