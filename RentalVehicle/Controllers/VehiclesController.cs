using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalVehicle.Data;
using RentalVehicle.Models;

namespace RentalVehicle.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly RentalDbContext _context;

        public VehiclesController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public IActionResult Index()
        {
            var rentalDbContext = _context.Vehicles
                .Include(v => v.FuleType)
                .Include(v => v.VehicleCategories)
                .OrderByDescending(r => r.VehicleID)
                .Take(9)
                .ToList();
            return View(rentalDbContext.ToList());
        }

        [HttpPost]
        public IActionResult Index(string search, int? minPrice=20, int? maxPrice=400)
        {
            if (search != null)
            {
                var vehicles = _context.Vehicles
                        .Include(v => v.FuleType)
                        .Include(v => v.VehicleCategories)
                        .Where(v => v.Make.Contains(search) || v.Model.Contains(search) || v.PlateNumber.ToString().Contains(search))
                        .ToList();
                return View(vehicles);
            }
            else if(minPrice.HasValue && maxPrice.HasValue)
            {
                var vehicles = _context.Vehicles
                        .Include(v => v.FuleType)
                        .Include(v => v.VehicleCategories).Where(v => v.RentalPricePerDay >= minPrice && v.RentalPricePerDay <= maxPrice)
                        .ToList(); 
                return View(vehicles);
            }
            else
            {
                var rentalDbContext = _context.Vehicles
                        .Include(v => v.FuleType)
                        .Include(v => v.VehicleCategories)
                        .OrderByDescending(r => r.VehicleID)
                        .ToList(); 
                return View(rentalDbContext.ToList());
            }
        }

        // GET: Vehicles/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = _context.Vehicles
                .Include(v => v.FuleType)
                .Include(v => v.VehicleCategories)
                .FirstOrDefault(m => m.VehicleID == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    ViewData["FuleTypeID"] = new SelectList(_context.FuleTypes, "FuleID", "FuleType");
                    ViewData["CarCategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
                    return View();
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vehicle vehicle, IFormFile Image)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (_context.Vehicles.Any(v => v.PlateNumber == vehicle.PlateNumber))
                    {
                        ModelState.AddModelError("PlateNumber", "Plate number already exists.");
                    }
                    if (ModelState.IsValid)
                    {
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/VehiclesImage", Image.FileName);
                        FileStream file = new FileStream(savePath, FileMode.Create);
                        Image.CopyTo(file);
                        file.Close();
                        vehicle.Image = Image.FileName;
                        _context.Add(vehicle);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["FuleTypeID"] = new SelectList(_context.FuleTypes, "FuleID", "FuleType", vehicle.FuleTypeID);
                    ViewData["CarCategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", vehicle.CarCategoryID);
                    return View(vehicle);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: Vehicles/Edit/5
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicle = _context.Vehicles.Find(id);
                    if (vehicle == null)
                    {
                        return NotFound();
                    }
                    ViewData["FuleTypeID"] = new SelectList(_context.FuleTypes, "FuleID", "FuleType", vehicle.FuleTypeID);
                    ViewData["CarCategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", vehicle.CarCategoryID);
                    return View(vehicle);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Vehicle vehicle, IFormFile Image)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id != vehicle.VehicleID)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        DeleteVehicleImage(vehicle.Image);
                        if (_context.Vehicles.Any(v => v.PlateNumber == vehicle.PlateNumber))
                        {
                            ModelState.AddModelError("PlateNumber", "Plate number already exists.");
                        }
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/VehiclesImage", Image.FileName);
                        FileStream file = new FileStream(savePath, FileMode.Create);
                        Image.CopyTo(file);
                        file.Close();
                        vehicle.Image = Image.FileName;
                        try
                        {
                            _context.Update(vehicle);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!VehicleExists(vehicle.VehicleID))
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
                    ViewData["FuleTypeID"] = new SelectList(_context.FuleTypes, "FuleID", "FuleType", vehicle.FuleTypeID);
                    ViewData["CarCategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", vehicle.CarCategoryID);
                    return View(vehicle);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
            
        }

        // GET: Vehicles/Delete/5
        public IActionResult Delete(int? id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if(HttpContext.Session.GetString("Role") == "Admin"){
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicle =_context.Vehicles
                        .Include(v => v.FuleType)
                        .Include(v => v.VehicleCategories)
                        .FirstOrDefault(m => m.VehicleID == id);
                    if (vehicle == null)
                    {
                        return NotFound();
                    }

                    return View(vehicle);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var vehicle = _context.Vehicles.Find(id);
                    if (vehicle != null)
                    {
                        DeleteVehicleImage(vehicle.Image);
                        _context.Vehicles.Remove(vehicle);
                    }
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("AccessDenied");
                }
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
            
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleID == id);
        }

        private void DeleteVehicleImage(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/VehiclesImage", imageName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
