﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics.Metrics;
using static Pipelines.Sockets.Unofficial.SocketConnection;

namespace Booking.Web.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IScheduleRepository _scheduleRepository;
        public PackagesController(IPackageRepository packageRepository,IScheduleRepository scheduleRepository)
        {
            _packageRepository = packageRepository;
            _scheduleRepository = scheduleRepository;
        }
        private int? GetUserId()
        {
            var userid = String.Empty;
            var cookie = Request.Cookies.TryGetValue(Constants.UserIdCookie, out userid);
            if (cookie)
                return int.Parse(userid);
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> IndexAsync(string country)
        {
            // The Country parameter will contain the selected item's value
            // Perform any necessary actions with the selected value

            return View(await _packageRepository.GetAllPackagesByCountry((Country)int.Parse(country),(int)GetUserId()));
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
            return View(await _packageRepository.GetAllPackagesByCountry((Country)0, (int)GetUserId()));
        }
        // GET: Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pid = (int)id;
            var package = await _packageRepository.GetPackage(pid);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // GET: Packages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Country,Fee,StartTime,EndTime")] CreatePackageVM package)
        {
            if (ModelState.IsValid)
            {
                await _packageRepository.Create(package);
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }
        public async Task<IActionResult> ScheduleAsync(int? id)
        {
            var userid = String.Empty;
            var cookie = Request.Cookies.TryGetValue(Constants.UserIdCookie,out userid);
            await _scheduleRepository.CreateBooking(new CreateScheduleVM
            {
                Created = DateTime.UtcNow,
                PackageId = (int)id,
                UserId = int.Parse(userid),
            });
            return View();
        }        
    }
}
