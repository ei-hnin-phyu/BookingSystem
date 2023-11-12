using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Booking.Web.Views.Users;

namespace Booking.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
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
                return RedirectToAction("Login", "Users");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            // Display the login form
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel)
        {
            // Validate the username and password. You can use your authentication logic here.

            if (this._userRepository.IsValidUser(loginModel.Email, loginModel.Password))
            {
                AddCookie(await this._userRepository.GetUserId(loginModel.Email));
                return RedirectToAction("Index", "Packages", new { });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
                return View();
            }
        }
        private void AddCookie(int id)
        {
            Response.Cookies.Append(Constants.UserIdCookie, id.ToString(), new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
            });
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
              return View(await this._userRepository.GetAllUsers());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userid = (int)id;
            var user = await this._userRepository.GetUser(userid);
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userid = String.Empty;
            Request.Cookies.TryGetValue(Constants.UserIdCookie, out userid);
            if (userid == null)
            {
                return NotFound();
            }
            var user = await this._userRepository.GetUser(int.Parse(userid));
            if(user.Packages == null)
            {
                user.Packages = new List<Package>();
            }
            var userprofile = new ProfileModel
            {
                Email = user.Email,
                NumberOfCredits = user.NumberOfCredits,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Packages = user.Packages,
                Schedules = user.Schedules
            };
            return View(userprofile);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,UserName,Password,NumberOfCredits,Country,PhoneNumber")] CreateUserVM user)
        {
            if (ModelState.IsValid)
            {
                await this._userRepository.Create(new CreateUserVM
                {
                    Email = user.Email,
                    NumberOfCredits = user.NumberOfCredits,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber
                });
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userid = (int)id;
            var user = await this._userRepository.GetUser(userid);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,UserName,Password,NumberOfCredits,Country,PhoneNumber")] EditUserVM user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this._userRepository.Update(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userid = (int)id;
            var user = await this._userRepository.GetUser(userid);

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return Problem("Entity set 'BookingDbContext.Users'  is null.");
            }
            await this._userRepository.Delete(id);           
            return RedirectToAction(nameof(Index));
        }
    }
}
