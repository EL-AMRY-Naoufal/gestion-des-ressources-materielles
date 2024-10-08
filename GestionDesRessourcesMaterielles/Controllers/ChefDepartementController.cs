﻿using GestionDesRessourcesMaterielles.Data;
using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDesRessourcesMaterielles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefDepartementController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;

        public ChefDepartementController(ApplicationDbContext applicationDbContext)
        {
            _authContext = applicationDbContext;
        }

        [HttpPost("sendDemandeBesoinToEnseignants")]
        public async Task<IActionResult> SendDemandeBesoinToEnseignants(int departementId)
        {
            try
            {
                var enseignants = await _authContext.PersonneDepartements
                                .Where(pd => pd.Role == 0 && pd.Departement.DepartmentId == departementId)
                                .ToListAsync();

                foreach (var enseignant in enseignants)
                {
                    enseignant.CanRequestResources = true;

                    // Save changes to the database
                    await _authContext.SaveChangesAsync();
                }
                return Ok(new
                {
                    Message = "Demands sent to enseignants successfully by the chef."
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Message = $"An error occurred while sending the demande: {ex.Message}"
                });
            }
        }

        [HttpPost("approveBesoins")]
        public async Task<IActionResult> ApproveBesoins([FromBody] ApprovalRequestModel model)
        {
            try
            {
                var chefDepartement = await _authContext.ChefDepartements.FirstOrDefaultAsync(c => c.UserId == model.UserId);
                if (chefDepartement == null)
                {
                    return NotFound("Chef Departement not found");
                }

                var besoins = await _authContext.Besoins.Where(b => model.BesoinIds.Contains(b.BesoinId)).ToListAsync();
                if (besoins == null || !besoins.Any())
                {
                    return NotFound("No besoins found to approve");
                }

                foreach (var besoin in besoins)
                {
                    besoin.IsSentByChefDepartement = true;
                }

                await _authContext.SaveChangesAsync();

                return Ok("Besoins approved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("GetBesoinsForChefDepartement")]
        public async Task<ActionResult<List<Besoin>>> GetBesoinsForChefDepartement(int userId)
        {
            try
            {
                // Find the ChefDepartement based on the userId
                var chefDepartement = await _authContext.ChefDepartements
                    .Include(cd => cd.Departement)
                    .FirstOrDefaultAsync(cd => cd.UserId == userId);

                if (chefDepartement == null)
                {
                    return NotFound($"ChefDepartement with UserId {userId} not found");
                }

                // Retrieve the besoins associated with the ChefDepartement's Departement
                var besoins = await _authContext.Besoins
                    .Where(b => b.PersonneDepartementId.Departement.DepartmentId == chefDepartement.Departement.DepartmentId && b.IsSentByChefDepartement == null)
                    .Include(b => b.PersonneDepartementId) // Include PersonneDepartementId
                    .Include(b => b.RessourceCatalogteId) // Include RessourceCatalogteId
                    .ToListAsync();

                // Return the list of besoins
                return Ok(besoins);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error if an exception occurs
                return StatusCode(500, $"An error occurred while fetching besoins: {ex.Message}");
            }
        }

        [HttpGet("GetResourcesForDepartement")]
        public async Task<ActionResult> GetResourcesForDepartement(int departementId)
        {
            try
            {
                // Find the department based on the departmentId
                var department = await _authContext.Departements.FindAsync(departementId);
                if (department == null)
                {
                    return NotFound($"Department with ID {departementId} not found");
                }

                // Retrieve all Ordinateurs associated with the department
                var ordinateurs = await _authContext.Ordinateurs
                    .Where(o => o.Departement.DepartmentId == departementId && o.Livred == true)
                    .Include(o => o.PersonneDepartement) // Include PersonneDepartement
                    .ToListAsync();

                // Retrieve all Imprimantes associated with the department
                var imprimantes = await _authContext.Imprimantes
                    .Where(i => i.Departement.DepartmentId == departementId && i.Livred == true)
                    .Include(i => i.PersonneDepartement) // Include PersonneDepartement
                    .ToListAsync();

                // You can process the ordinateurs and imprimantes lists here as needed
                // For example, you can return them as JSON objects

                return Ok(new { Ordinateurs = ordinateurs, Imprimantes = imprimantes });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error if an exception occurs
                return StatusCode(500, $"An error occurred while fetching resources: {ex.Message}");
            }
        }



    }
    public class ApprovalRequestModel
    {
        public int UserId { get; set; }
        public List<int> BesoinIds { get; set; }
    }

}
