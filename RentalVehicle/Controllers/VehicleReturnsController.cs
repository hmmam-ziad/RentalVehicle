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
    public class VehicleReturnsController : Controller
    {
        private readonly RentalDbContext _context;

        public VehicleReturnsController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: VehicleReturns
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var rentalDbContext = _context.VehicleReturns.Include(v => v.RentalTransaction);
                    return View(rentalDbContext.ToList());
                }
            }
            return View("AccessDenied");
        }

        // GET: VehicleReturns/Details/5
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

                    var vehicleReturn = _context.VehicleReturns
                        .Include(v => v.RentalTransaction)
                        .FirstOrDefault(m => m.ReturnID == id);
                    if (vehicleReturn == null)
                    {
                        return NotFound();
                    }

                    return View(vehicleReturn);
                }
            }
            return View("AccessDenied");
        }

        // GET: VehicleReturns/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    ViewData["TransactionID"] = new SelectList(_context.RentalTransactions, "TransactionID", "TransactionID");
                    return View();
                }
            }
            return View("AccessDenied");
        }

        // POST: VehicleReturns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VehicleReturn vehicleReturn)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (!ModelState.IsValid)
                    {
                        CalculateVehicleReturnDetails(vehicleReturn);
                        ProcessVehicleReturn(vehicleReturn);
                        UpdateVehicleMileageAndISAvailabel(vehicleReturn);
                        _context.Add(vehicleReturn);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["TransactionID"] = new SelectList(_context.RentalTransactions, "TransactionID", "TransactionID", vehicleReturn.TransactionID);
                    return View(vehicleReturn);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: VehicleReturns/Edit/5
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

                    var vehicleReturn = _context.VehicleReturns.Find(id);
                    if (vehicleReturn == null)
                    {
                        return NotFound();
                    }
                    ViewData["TransactionID"] = new SelectList(_context.RentalTransactions, "TransactionID", "TransactionID", vehicleReturn.TransactionID);
                    return View(vehicleReturn);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: VehicleReturns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VehicleReturn vehicleReturn)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id != vehicleReturn.ReturnID)
                    {
                        return NotFound();
                    }

                    if (!ModelState.IsValid)
                    {

                        try
                        {
                            CalculateVehicleReturnDetails(vehicleReturn);
                            ProcessVehicleReturn(vehicleReturn);
                            UpdateVehicleMileageAndISAvailabel(vehicleReturn);
                            _context.Update(vehicleReturn);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!VehicleReturnExists(vehicleReturn.ReturnID))
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
                    ViewData["TransactionID"] = new SelectList(_context.RentalTransactions, "TransactionID", "TransactionID", vehicleReturn.TransactionID);
                    return View(vehicleReturn);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: VehicleReturns/Delete/5
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicleReturn = _context.VehicleReturns
                        .Include(v => v.RentalTransaction)
                        .FirstOrDefault(m => m.ReturnID == id);
                    if (vehicleReturn == null)
                    {
                        return NotFound();
                    }

                    return View(vehicleReturn);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: VehicleReturns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var vehicleReturn = _context.VehicleReturns.Find(id);
                    if (vehicleReturn != null)
                    {
                        _context.VehicleReturns.Remove(vehicleReturn);
                    }

                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        private bool VehicleReturnExists(int id)
        {
            return _context.VehicleReturns.Any(e => e.ReturnID == id);
        }

        private void CalculateVehicleReturnDetails(VehicleReturn vehicleReturn)
        {
            var transaction = _context.RentalTransactions
                .Include(rt => rt.RentalBooking)
                .ThenInclude(rb => rb.Vehicles)
                .FirstOrDefault(rt => rt.TransactionID == vehicleReturn.TransactionID);

            if (transaction != null)
            {
                vehicleReturn.ConsumedMileage = vehicleReturn.Mileage + transaction.RentalBooking.Vehicles.Mileage;
                vehicleReturn.ActualTotalDueAmount = transaction.ActualTotalDueAmount + vehicleReturn.AdditionalCharges;
            }
        }

        private void ProcessVehicleReturn(VehicleReturn vehicleReturn)
        {

            var rentalTransaction = _context.RentalTransactions
                .Include(rt => rt.RentalBooking)
                .ThenInclude(rb => rb.Vehicles)
                .FirstOrDefault(rt => rt.TransactionID == vehicleReturn.TransactionID);

            if (rentalTransaction != null && rentalTransaction.RentalBooking != null)
            {
                var rentalBooking = rentalTransaction.RentalBooking;

                if (vehicleReturn.ActualReturnDate != null && rentalBooking.RentalEndDate != null)
                {
                    if (vehicleReturn.ActualReturnDate > rentalBooking.RentalEndDate)
                    {

                        int extraDays = (vehicleReturn.ActualReturnDate - rentalBooking.RentalEndDate).Days;
                        vehicleReturn.AdditionalCharges = extraDays * rentalBooking.Vehicles.RentalPricePerDay;
                    }
                    else if (vehicleReturn.ActualReturnDate < rentalBooking.RentalEndDate)
                    {
                        int unusedDays = (rentalBooking.RentalEndDate - vehicleReturn.ActualReturnDate).Days;
                        decimal discount = unusedDays * rentalBooking.Vehicles.RentalPricePerDay;
                        decimal totalDueAfterDiscount = vehicleReturn.ActualTotalDueAmount - discount;
                        vehicleReturn.ActualTotalDueAmount = totalDueAfterDiscount;
                    }
                }
            }
        }

        private void UpdateVehicleMileageAndISAvailabel(VehicleReturn vehicleReturn)
        {
            var rentalTransaction = _context.RentalTransactions
                .Include(rt => rt.RentalBooking)
                .ThenInclude(rb => rb.Vehicles)
                .FirstOrDefault(rt => rt.TransactionID == vehicleReturn.TransactionID);
            if (rentalTransaction != null && rentalTransaction.RentalBooking != null)
            {
                var rentalBooking = rentalTransaction.RentalBooking;
                rentalBooking.Vehicles.Mileage = vehicleReturn.ConsumedMileage;
                rentalBooking.Vehicles.ISAvailabelForRent = true;
                _context.Update(rentalBooking.Vehicles);
                _context.SaveChanges();
            }
        }

    }
}
