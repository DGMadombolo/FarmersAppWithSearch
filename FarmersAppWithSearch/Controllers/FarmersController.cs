using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarmersAppWithSearch.Data;
using FarmersAppWithSearch.Models;
using BlobStorageMVC.Services;

namespace FarmersAppWithSearch.Controllers
{
    public class FarmersController : Controller
    {
        private readonly ApplicationDBContext _context;

        //Add Blob service
         private readonly BlobService _blobService;

        public FarmersController(ApplicationDBContext context, BlobService blobService)

        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Farmers
        public async Task<IActionResult> Index(string searchString)
        {
            //Get all farmers from the database
            var allFarmers = await _context.Farmers.ToListAsync();

            //If no search text, return all
            if(string.IsNullOrEmpty(searchString))
            {
                return View(allFarmers);
            }

            //Create new list for filtered results
            List<Farmer> filteredFarmers = new List<Farmer>();

            //loop through farmers manually
            foreach(var farmer in allFarmers)
            {
                //Check if any field contains the search text
                if((farmer.FullName != null && farmer.FullName.Contains(searchString)) ||
                   (farmer.FarmerCode != null && farmer.FarmerCode.Contains(searchString)) ||
                   (farmer.FarmName != null && farmer.FarmName.Contains(searchString)) ||
                   (farmer.Location != null && farmer.Location.Contains(searchString)))
                {
                    filteredFarmers.Add(farmer);
                }
            }
            //return filtered list
            return View(filteredFarmers);
        }

        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: Farmers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Farmers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,FarmerCode,PhoneNumber,FarmName,Location,ImageUrl")] Farmer farmer, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {    
                //Add if statement to handle blob image
                if (imageFile != null && imageFile.Length > 0)
                {
                    farmer.ImageUrl = await _blobService.UploadImageAync(imageFile);
                }

                _context.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        // GET: Farmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,FarmerCode,PhoneNumber,FarmName,Location,ImageUrl")] Farmer farmer, IFormFile imageFile)
        {
            if (id != farmer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        farmer.ImageUrl = await _blobService.UploadImageAync(imageFile);
                    }
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerExists(int id)
        {
            return _context.Farmers.Any(e => e.Id == id);
        }
    }
}
