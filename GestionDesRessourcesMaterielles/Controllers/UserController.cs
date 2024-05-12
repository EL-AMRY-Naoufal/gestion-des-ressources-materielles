using GestionDesRessourcesMaterielles.Data;
using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GestionDesRessourcesMaterielles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            _authContext = applicationDbContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserCredential userCredential)
        {
            if (userCredential == null)
                return BadRequest();

            var chefDepartement = await _authContext.ChefDepartements.FirstOrDefaultAsync(u => u.Email == userCredential.Email);
            if (chefDepartement != null)
            {
                return Ok(new
                {
                    Token = CreateJwtToken(chefDepartement),
                    Message = "Login Success",
                    User = chefDepartement 
                });
            }

            var fournisseur = await _authContext.Fournisseurs.FirstOrDefaultAsync(u => u.Email == userCredential.Email);
            if (fournisseur != null)
            {
                return Ok(new
                {
                    Token = CreateJwtToken(fournisseur),
                    Message = "Login Success",
                    User = fournisseur 
                });
            }

            var personneDepartement = await _authContext.PersonneDepartements.FirstOrDefaultAsync(u => u.Email == userCredential.Email);
            if (personneDepartement != null)
            {
                return Ok(new
                {
                    Token = CreateJwtToken(personneDepartement),
                    Message = "Login Success",
                    User = personneDepartement
                });
            }

            var responsableRessources = await _authContext.ResponsableRessources.FirstOrDefaultAsync(u => u.Email == userCredential.Email);
            if (responsableRessources != null)
            {
                return Ok(new
                {
                    Token = CreateJwtToken(responsableRessources),
                    Message = "Login Success",
                    User = responsableRessources 
                });
            }

            var serviceMaintenance = await _authContext.ServiceMaintenances.FirstOrDefaultAsync(u => u.Email == userCredential.Email);
            if (serviceMaintenance != null)
            {
                return Ok(new
                {
                    Token = CreateJwtToken(serviceMaintenance),
                    Message = "Login Success",
                    User = serviceMaintenance 
                });
            }

            return NotFound(new { Message = "User not found!" });
        }
        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }

        private string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecretkey1234567890123456");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Email),
                new Claim(ClaimTypes.Name, $"{user.Email} {user.Password}")
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [HttpPost("registerFournisseur")]
        public async Task<IActionResult> RegisterFournisseur([FromBody] RegisterFournisseurModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authContext.Fournisseurs.AnyAsync(u => u.Email == model.Email))
            {
                return Conflict(new { Message = "Email is already registered" });
            }

            var passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(null, model.Password);

            var fournisseur = new Fournisseur
            {
                Email = model.Email,
                Password = hashedPassword,
                Name = model.Name,
                NomSociete = model.NomSociete,
                Lieu = model.Lieu,
                Gerant = model.Gerant
            };

            _authContext.Fournisseurs.Add(fournisseur);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Fournisseur registered successfully" });
        }

        [HttpPost("registerChefDepartement")]
        public async Task<IActionResult> RegisterChefDepartement([FromBody] RegisterChefDepartementModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authContext.ChefDepartements.AnyAsync(u => u.Email == model.Email))
            {
                return Conflict(new { Message = "Email is already registered" });
            }

            var passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(null, model.Password);

            // Retrieve the Departement from the database based on the provided DepartementId
            var departement = await _authContext.Departements.FindAsync(model.DepartementId);
            if (departement == null)
            {
                return NotFound(new { Message = "Departement not found" });
            }

            var chefDepartement = new ChefDepartement
            {
                Email = model.Email,
                Password = hashedPassword,
                Name = model.Name,
                Departement = departement // Assign the retrieved Departement
            };

            _authContext.ChefDepartements.Add(chefDepartement);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Chef Departement registered successfully" });
        }

        [HttpPost("registerPersonneDepartement")]
        public async Task<IActionResult> RegisterPersonneDepartement([FromBody] RegisterPersonneDepartementModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authContext.PersonneDepartements.AnyAsync(u => u.Email == model.Email))
            {
                return Conflict(new { Message = "Email is already registered" });
            }

            var passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(null, model.Password);

            // Retrieve the Departement from the database based on the provided DepartementId
            var departement = await _authContext.Departements.FindAsync(model.DepartementId);
            if (departement == null)
            {
                return NotFound(new { Message = "Departement not found" });
            }

            var personneDepartement = new PersonneDepartement
            {
                Email = model.Email,
                Password = hashedPassword,
                Name = model.Name,
                Role = model.Role,
                Laboratoire = model.Laboratoire,
                Departement = departement // Assign the retrieved Departement
            };

            _authContext.PersonneDepartements.Add(personneDepartement);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Personne Departement registered successfully" });
        }
    }

    public class UserCredential
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterFournisseurModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NomSociete { get; set; }
        public string Lieu { get; set; }
        public string Gerant { get; set; }
    }

    public class RegisterChefDepartementModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int DepartementId { get; set; } 
    }

    public class RegisterPersonneDepartementModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public string Laboratoire { get; set; }
        public int DepartementId { get; set; }
    }
}
