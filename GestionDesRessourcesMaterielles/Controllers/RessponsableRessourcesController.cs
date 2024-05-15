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
        public async Task<IActionResult> HandleOffreFournisseur(int offreFournisseurId, bool accept)
        {
            try
            {
                var offreFournisseur = await _authContext.offreFournisseurs
                                             .Include(o => o.AppelOffreId) // Include the AppelOffre navigation property
                                                 .ThenInclude(a => a.Departement) // Include the Departement navigation property within AppelOffre
                                             .FirstOrDefaultAsync(o => o.OffreFournisseurId == offreFournisseurId);
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

                if (accept && departement.Budget < offreFournisseur.Montant)
                {
                    offreFournisseur.IsAccepted = false;
                    return BadRequest("Department budget is not sufficient for this offer");
                }

                // Update the acceptance status
                offreFournisseur.IsAccepted = accept;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok($"Offer {offreFournisseurId} {(accept ? "accepted" : "rejected")} successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

