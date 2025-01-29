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
    public class VehicleCategoriesController : Controller
    {
        private readonly RentalDbContext _context;

        public VehicleCategoriesController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: VehicleCategories
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    return View(_context.Categories.ToList());
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: VehicleCategories/Details/5
        public IActionResult Details(int? id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicleCategories = _context.Categories
                        .FirstOrDefault(m => m.CategoryID == id);
                    if (vehicleCategories == null)
                    {
                        return NotFound();
                    }

                    return View(vehicleCategories);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: VehicleCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VehicleCategories vehicleCategories)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(vehicleCategories);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(vehicleCategories);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: VehicleCategories/Edit/5
        public IActionResult Edit(int? id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicleCategories = _context.Categories.Find(id);
                    if (vehicleCategories == null)
                    {
                        return NotFound();
                    }
                    return View(vehicleCategories);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: VehicleCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,VehicleCategories vehicleCategories)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id != vehicleCategories.CategoryID)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(vehicleCategories);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!VehicleCategoriesExists(vehicleCategories.CategoryID))
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
                    return View(vehicleCategories);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: VehicleCategories/Delete/5
        public IActionResult Delete(int? id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicleCategories = _context.Categories
                        .FirstOrDefault(m => m.CategoryID == id);
                    if (vehicleCategories == null)
                    {
                        return NotFound();
                    }

                    return View(vehicleCategories);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: VehicleCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                var vehicleCategories = _context.Categories.Find(id);
                if (vehicleCategories != null)
                {
                    _context.Categories.Remove(vehicleCategories);
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Login", "Customer");
        }

        private bool VehicleCategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
