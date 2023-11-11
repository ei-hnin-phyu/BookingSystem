using Booking.Web.Repository.Interface;
using Booking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking.Web.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IJWTAuthService _jwtService;
        public AccountController(IConfiguration configuration,IUserRepository userRepository,IJWTAuthService authService) { 
            this._configuration = configuration;
            this._userRepository = userRepository;
            this._jwtService = authService;
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

            if (this._userRepository.IsValidUser(username, password))
            {
                var token = GenerateBearerToken(username);
                return RedirectToAction("Index", "Users", new { token = token });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
                return View();
            }
        }

        private string GenerateBearerToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };
            return this._jwtService.CreateJwtSecurityToken(claims);
        }
    }

}
