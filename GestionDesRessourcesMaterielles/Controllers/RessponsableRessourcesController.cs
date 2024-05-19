using GestionDesRessourcesMaterielles.Data;
using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GestionDesRessourcesMaterielles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RessponsableRessourcesController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;

        public RessponsableRessourcesController(ApplicationDbContext applicationDbContext)
        {
            _authContext = applicationDbContext;
        }

        [HttpGet("besoins")]
        public async Task<IActionResult> GetBesoinsForResponsableRessources()
        {
            try
            {
                var besoins = await _authContext.Besoins
                    .Where(b => b.IsSentByChefDepartement == true && b.IsApprovedByResponsableRessource == null)
                    .Include(b => b.PersonneDepartementId) 
                    .Include(b => b.RessourceCatalogteId) 
                    .ToListAsync();

                return Ok(besoins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("createAppelOffre")]
        public async Task<IActionResult> CreateAppelOffre()
        {
            try
            {
                        var besoinsByDepartement = await _authContext.Besoins
                    .Where(b => b.IsSentByChefDepartement == true && b.IsApprovedByResponsableRessource == null)
                    .Include(b => b.PersonneDepartementId)
                        .ThenInclude(p => p.Departement) 
                    .GroupBy(b => b.PersonneDepartementId.Departement)
                    .ToListAsync();

                if (!besoinsByDepartement.Any())
                {
                    return BadRequest("No besoins to approve");
                }

                foreach (var group in besoinsByDepartement)
                {
                    var departementId = group.Key;
                    var besoinsToApprove = group.ToList();

                    var appelOffre = new AppelOffre
                    {
                        Departement = departementId,
                        Besoins = besoinsToApprove
                    };

                    _authContext.AppelOffres.Add(appelOffre);

                    foreach (var besoin in besoinsToApprove)
                    {
                        besoin.IsApprovedByResponsableRessource = true;
                        besoin.AppelOffre = appelOffre;
                    }
                }

                await _authContext.SaveChangesAsync();

                return Ok("All besoins approved and associated with AppelOffres");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("offresFournisseur")]
        public async Task<IActionResult> GetAllOffresFournisseur()
        {
            try
            {
                var offres = await _authContext.offreFournisseurs
                    .Where(o => o.IsAccepted == null) 
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

        [HttpPost("handleOffreFournisseur")]
        public async Task<IActionResult> HandleOffreFournisseur([FromBody] HandleOffreFournisseurDto dto)
        {
            try
            {
                var offreFournisseur = await _authContext.offreFournisseurs
                                             .Include(o => o.AppelOffreId) // Include the AppelOffre navigation property
                                                 .ThenInclude(a => a.Departement) // Include the Departement navigation property within AppelOffre
                                             .FirstOrDefaultAsync(o => o.AppelOffreId.AppelOffreID == dto.OffreFournisseurId);
                if (offreFournisseur == null)
                {
                    return NotFound("OffreFournisseur not found");
                }

                if (offreFournisseur.IsAccepted.HasValue)
                {
                    return BadRequest("Offer has already been accepted or rejected");
                }

                // Check if the department's budget is sufficient
                var appelOffre = offreFournisseur.AppelOffreId;
                if (appelOffre == null)
                {
                    return NotFound("AppelOffre not found");
                }

                var departement = appelOffre.Departement;
                if (departement == null)
                {
                    return NotFound("Departement not found");
                }

                if (dto.Accept && departement.Budget < offreFournisseur.Montant)
                {
                    offreFournisseur.IsAccepted = false;
                    return BadRequest("Department budget is not sufficient for this offer");
                }

                // Update the acceptance status
                offreFournisseur.IsAccepted = dto.Accept;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok($"Offer {dto.OffreFournisseurId} {(dto.Accept ? "accepted" : "rejected")} successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("offresFournisseurs")]
        public async Task<IActionResult> GetNonTreatedOffresFournisseur()
        {
            try
            {
                var appelOffres = await _authContext.AppelOffres
                    .Include(a => a.Departement)
                    .Include(a => a.Besoins)
                    .ThenInclude(b => b.RessourceCatalogteId)
                    .Where(a => _authContext.offreFournisseurs.Any(o => o.AppelOffreId.AppelOffreID == a.AppelOffreID && o.IsAccepted == null))
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
        public class AppelOffreDto
        {
            public int AppelOffreID { get; set; }
            public string DepartementName { get; set; }
            public List<OrdinateurBesoinDto> OrdinateurBesoins { get; set; }
            public List<ImprimanteBesoinDto> ImprimanteBesoins { get; set; }
            public bool? IsAccepted { get; set; }
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

        public class HandleOffreFournisseurDto
        {
            public int OffreFournisseurId { get; set; }
            public bool Accept { get; set; }
        }
    }

}

