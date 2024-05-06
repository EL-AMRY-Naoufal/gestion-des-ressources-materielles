using Azure.Identity;
using GestionDesRessourcesMaterielles.Data;
using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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

            User chefDepartementUser = await _authContext.ChefDepartements.FirstOrDefaultAsync(user => user.Email == userCredential.Email);
            User fournisseurUser = await _authContext.Fournisseurs.FirstOrDefaultAsync(user => user.Email == userCredential.Email);
            


            return Ok(new
            {
                Message = "Login Succes"
            });
        }

        private string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret......");
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
    }

    public class UserCredential
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
