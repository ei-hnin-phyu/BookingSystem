using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Booking.Web.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
        public AccountController(IConfiguration configuration) { 
            this._configuration = configuration;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            // Display the login form
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            // Validate the username and password. You can use your authentication logic here.

            if (IsValidUser(username, password))
            {
                // Once the user is authenticated, generate a Bearer token.
                var token = GenerateBearerToken(username);

                // Return the token to the client or store it as needed.
                return Ok(new { token });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
                return View();
            }
        }

        // ...

        private bool IsValidUser(string username, string password)
        {
            // Implement your own logic to validate the user's credentials here.
            // This is a basic example; you should use a more secure method.            
            return (username == "user" && password == "password");
        }
        public string GenerateBearerToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Set the token expiration time as needed
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
