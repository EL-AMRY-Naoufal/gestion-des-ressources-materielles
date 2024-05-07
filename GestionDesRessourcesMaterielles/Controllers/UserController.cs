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

            var users = new List<User>();
            users.AddRange(await _authContext.ChefDepartements.Where(u => u.Email == userCredential.Email).ToListAsync());
            users.AddRange(await _authContext.Fournisseurs.Where(u => u.Email == userCredential.Email).ToListAsync());
            users.AddRange(await _authContext.PersonneDepartements.Where(u => u.Email == userCredential.Email).ToListAsync());
            users.AddRange(await _authContext.ResponsableRessources.Where(u => u.Email == userCredential.Email).ToListAsync());
            users.AddRange(await _authContext.ServiceMaintenances.Where(u => u.Email == userCredential.Email).ToListAsync());

            var user = users.FirstOrDefault();

            Console.WriteLine(user.Password);

            if(user == null)
                return NotFound(new { Message = "User not found!" });

            if (!VerifyPassword(userCredential.Password, user.Password))
                return Unauthorized(new { Message = "Invalid password" });

            return Ok(new
            {
                Token = CreateJwtToken(user),
                Message = "Login Succes",
                User = user
            });
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

        [HttpPost("register")]
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
}
