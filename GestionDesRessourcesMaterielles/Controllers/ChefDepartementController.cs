using GestionDesRessourcesMaterielles.Data;
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
    }
}
