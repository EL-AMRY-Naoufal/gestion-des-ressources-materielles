using GestionDesRessourcesMaterielles.Data;
using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDesRessourcesMaterielles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonneDepartementController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;

        public PersonneDepartementController(ApplicationDbContext applicationDbContext)
        {
            _authContext = applicationDbContext;
        }

        [HttpPost("sendRequestForResources")]
        public async Task<IActionResult> SendRequestForResources([FromBody] ResourceRequestModel request)
        {
            try
            {
                var personne = await _authContext.PersonneDepartements.FirstOrDefaultAsync(p => p.UserId == request.PersonneId);

                if (personne == null)
                {
                    return NotFound("Person not found");
                }

                if (!personne.CanRequestResources)
                {
                    return BadRequest("You are not authorized to make resource requests");
                }

                foreach (var imprimanteRequest in request.ImprimanteRequests)
                {
                    string imprimanteId = imprimanteRequest.Key;
                    int numberOfBesoin = imprimanteRequest.Value;

                    var ressourceCatalog = await _authContext.ImprimanteCatalog.FirstOrDefaultAsync(rc => rc.RessourceCatalogID == imprimanteId);
                    if (ressourceCatalog == null)
                    {
                        return NotFound("Ressource imprimante not found");
                    }

                    var besoin = new Besoin
                    {
                        PersonneDepartementId = personne,
                        RessourceCatalogteId = ressourceCatalog, 
                        NumberOfRessource = numberOfBesoin,
                        DateRequested = DateTime.Now,
                        IsSentByChefDepartement = null, 
                        IsApprovedByResponsableRessource = null 
                    };

                    _authContext.Besoins.Add(besoin);
                }

                foreach (var ordinateurRequest in request.OrdinateurRequests)
                {
                    string ordinateurId = ordinateurRequest.Key;
                    int numberOfBesoin = ordinateurRequest.Value;

                    var ressourceCatalog = await _authContext.OrdinateurCatalog.FirstOrDefaultAsync(rc => rc.RessourceCatalogID == ordinateurId);
                    if (ressourceCatalog == null)
                    {
                        return NotFound("Ressource ordinateur not found");
                    }

                    var besoin = new Besoin
                    {
                        PersonneDepartementId = personne,
                        RessourceCatalogteId = ressourceCatalog, 
                        NumberOfRessource = numberOfBesoin,
                        DateRequested = DateTime.Now,
                        IsSentByChefDepartement = null, 
                        IsApprovedByResponsableRessource = null 
                    };

                    _authContext.Besoins.Add(besoin);
                }

                personne.CanRequestResources = false;

                await _authContext.SaveChangesAsync();
                return Ok("Resource requests sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

    }

    public class ResourceRequestModel
    {
        public int PersonneId { get; set; } 
        public Dictionary<string, int> ImprimanteRequests { get; set; } 
        public Dictionary<string, int> OrdinateurRequests { get; set; } 
    }
}
