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

namespace Booking.Web.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackageRepository _packageRepository;

        public PackagesController(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
            return View(await _packageRepository.GetAllPackages());
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

        // GET: Packages/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Country,Fee,StartTime,EndTime")] EditPackageVM package)
        {
            if (id != package.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _packageRepository.Update(package);
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }

        // GET: Packages/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _packageRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
