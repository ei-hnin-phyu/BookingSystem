using Booking.Web.Repository.Interface;
using Booking.Web.Views.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public AccountController(IConfiguration configuration,IUserRepository userRepository) { 
            this._configuration = configuration;
            this._userRepository = userRepository;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            // Display the login form
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.Create(new Models.CreateUserVM
                {
                    Email = model.Email,
                    Password = model.Password,
                });
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            // Display the login form
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginModel loginModel)
        {
            // Validate the username and password. You can use your authentication logic here.

            if (this._userRepository.IsValidUser(loginModel.Email,loginModel.Password))
            {
                return RedirectToAction("Index", "Users", new { });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
                return View();
            }
        }        
    }

}
