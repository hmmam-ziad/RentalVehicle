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
    public class RentalTransactionsController : Controller
    {
        private readonly RentalDbContext _context;

        public RentalTransactionsController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: RentalTransactions
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if(HttpContext.Session.GetString("Role") == "Admin")
                {
                    var rentalDbContext = _context.RentalTransactions.Include(r => r.RentalBooking);
                    return View(rentalDbContext.ToList());
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalTransactions/Details/5
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

                    var rentalTransaction = _context.RentalTransactions
                        .Include(r => r.RentalBooking)
                        .FirstOrDefault(m => m.TransactionID == id);
                    if (rentalTransaction == null)
                    {
                        return NotFound();
                    }

                    return View(rentalTransaction);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalTransactions/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    ViewData["BookingID"] = new SelectList(_context.RentalBookings, "BookingID", "BookingID");
                    return View();
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: RentalTransactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RentalTransaction rentalTransaction)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(rentalTransaction);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["BookingID"] = new SelectList(_context.RentalBookings, "BookingID", "BookingID", rentalTransaction.BookingID);
                    return View(rentalTransaction);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalTransactions/Edit/5
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

                    var rentalTransaction = _context.RentalTransactions.Find(id);
                    if (rentalTransaction == null)
                    {
                        return NotFound();
                    }
                    ViewData["BookingID"] = new SelectList(_context.RentalBookings, "BookingID", "BookingID", rentalTransaction.BookingID);
                    return View(rentalTransaction);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: RentalTransactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,RentalTransaction rentalTransaction)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id != rentalTransaction.TransactionID)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(rentalTransaction);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!RentalTransactionExists(rentalTransaction.TransactionID))
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
                    ViewData["BookingID"] = new SelectList(_context.RentalBookings, "BookingID", "BookingID", rentalTransaction.BookingID);
                    return View(rentalTransaction);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalTransactions/Delete/5
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

                    var rentalTransaction = _context.RentalTransactions
                        .Include(r => r.RentalBooking)
                        .FirstOrDefault(m => m.TransactionID == id);
                    if (rentalTransaction == null)
                    {
                        return NotFound();
                    }

                    return View(rentalTransaction);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: RentalTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var rentalTransaction = _context.RentalTransactions.Find(id);
                    if (rentalTransaction != null)
                    {
                        _context.RentalTransactions.Remove(rentalTransaction);
                    }

                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        private bool RentalTransactionExists(int id)
        {
            return _context.RentalTransactions.Any(e => e.TransactionID == id);
        }
    }
}
