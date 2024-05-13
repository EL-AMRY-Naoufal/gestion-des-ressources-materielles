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
    }
}
