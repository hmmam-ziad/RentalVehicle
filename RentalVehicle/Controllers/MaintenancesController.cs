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
    public class MaintenancesController : Controller
    {
        private readonly RentalDbContext _context;

        public MaintenancesController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: Maintenances
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var rentalDbContext = _context.Maintenances.Include(m => m.Vehicle);
                    return View(rentalDbContext.ToList());
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: Maintenances/Details/5
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

                    var maintenance = _context.Maintenances
                        .Include(m => m.Vehicle)
                        .FirstOrDefault(m => m.MaintenanceID == id);
                    if (maintenance == null)
                    {
                        return NotFound();
                    }

                    return View(maintenance);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: Maintenances/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    ViewData["VehicleID"] = new SelectList(_context.Vehicles, "VehicleID", "Model");
                    return View();
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: Maintenances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Maintenance maintenance)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(maintenance);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["VehicleID"] = new SelectList(_context.Vehicles, "VehicleID", "Model", maintenance.VehicleID);
                    return View(maintenance);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: Maintenances/Edit/5
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

                    var maintenance = _context.Maintenances.Find(id);
                    if (maintenance == null)
                    {
                        return NotFound();
                    }
                    ViewData["VehicleID"] = new SelectList(_context.Vehicles, "VehicleID", "Model", maintenance.VehicleID);
                    return View(maintenance);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: Maintenances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Maintenance maintenance)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id != maintenance.MaintenanceID)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(maintenance);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!MaintenanceExists(maintenance.MaintenanceID))
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
                    ViewData["VehicleID"] = new SelectList(_context.Vehicles, "VehicleID", "Model", maintenance.VehicleID);
                    return View(maintenance);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: Maintenances/Delete/5
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

                    var maintenance = _context.Maintenances
                        .Include(m => m.Vehicle)
                        .FirstOrDefault(m => m.MaintenanceID == id);
                    if (maintenance == null)
                    {
                        return NotFound();
                    }

                    return View(maintenance);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var maintenance = _context.Maintenances.Find(id);
                    if (maintenance != null)
                    {
                        _context.Maintenances.Remove(maintenance);
                    }

                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        private bool MaintenanceExists(int id)
        {
            return _context.Maintenances.Any(e => e.MaintenanceID == id);
        }
    }
}
