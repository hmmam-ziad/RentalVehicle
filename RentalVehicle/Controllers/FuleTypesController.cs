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
    public class FuleTypesController : Controller
    {
        private readonly RentalDbContext _context;

        public FuleTypesController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: FuleTypes
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    return View(_context.FuleTypes.ToList());
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        // GET: FuleTypes/Details/5
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();

                    }
                    var fuleTypes = _context.FuleTypes
                        .FirstOrDefault(m => m.FuleID == id);
                    if (fuleTypes == null)
                    {
                        return NotFound();
                    }

                    return View(fuleTypes);
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        // GET: FuleTypes/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    return View(); 
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        // POST: FuleTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FuleTypes fuleTypes)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(fuleTypes);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(fuleTypes);
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        // GET: FuleTypes/Edit/5
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

                    var fuleTypes = _context.FuleTypes.Find(id);
                    if (fuleTypes == null)
                    {
                        return NotFound();
                    }
                    return View(fuleTypes);
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
           
        }

        // POST: FuleTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,FuleTypes fuleTypes)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id != fuleTypes.FuleID)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(fuleTypes);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!FuleTypesExists(fuleTypes.FuleID))
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
                    return View(fuleTypes);
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
            
        }

        // GET: FuleTypes/Delete/5
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
                    var fuleTypes = _context.FuleTypes
                        .FirstOrDefault(m => m.FuleID == id);
                    if (fuleTypes == null)
                    {
                        return NotFound();
                    }
                    return View(fuleTypes);
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        // POST: FuleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var fuleTypes = _context.FuleTypes.Find(id);
                    if (fuleTypes != null)
                    {
                        _context.FuleTypes.Remove(fuleTypes);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View("AccessDenied");
            }
            else
                return RedirectToAction("Login", "Customer");
        }

        private bool FuleTypesExists(int id)
        {
            return _context.FuleTypes.Any(e => e.FuleID == id);
        }
    }
}
