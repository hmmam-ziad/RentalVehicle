using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalVehicle.Data;
using RentalVehicle.Models;

namespace RentalVehicle.Controllers
{
    public class RentalBookingsController : Controller
    {
        private readonly RentalDbContext _context;

        public RentalBookingsController(RentalDbContext context)
        {
            _context = context;
        }

        // GET: RentalBookings
        public IActionResult Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");
            var customerRole = HttpContext.Session.GetString("Role");

            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                var rentalDbContext = _context.RentalBookings
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicles);
                return View(rentalDbContext.ToList().Where(m => m.CustomerID == customerId || customerRole == "Admin"));
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalBookings/Details/5
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                var customerID = HttpContext.Session.GetInt32("CustomerID");

                if (id == null)
                {
                    return NotFound();
                }

                var rentalBooking = _context.RentalBookings
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicles)
                    .FirstOrDefault(m => m.BookingID == id && m.CustomerID == customerID);

                if (rentalBooking == null)
                {
                    return NotFound("Booking not found or you do not have access to it.");
                }

                return View(rentalBooking);
            }

            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalBookings/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                var customerId = HttpContext.Session.GetInt32("CustomerID");
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == customerId);

                if (customerId != null)
                {
                    ViewBag.CutomerName = customer?.CustomerName;
                }

                ViewBag.AvailableVehicles = _context.Vehicles.ToList(); // عرض جميع المركبات
                return View();
            }
            return RedirectToAction("Login", "Customer");
        }

        // POST: RentalBookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RentalBooking rentalBooking)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                BookingValidation(rentalBooking);
                CalculationDays(rentalBooking);

                var customerId = HttpContext.Session.GetInt32("CustomerID");
                rentalBooking.CustomerID = customerId.Value;

                if (!ModelState.IsValid)
                {
                    _context.Add(rentalBooking);
                    _context.SaveChanges();
                    return RedirectToAction("Details", new { id = rentalBooking.BookingID });
                }

                ViewBag.AvailableVehicles = _context.Vehicles.ToList();
                return View(rentalBooking);
            }
            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalBookings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                var customerID = HttpContext.Session.GetInt32("CustomerID");
                var customerRole = HttpContext.Session.GetString("Role");

                if (id == null)
                {
                    return NotFound();
                }

                var rentalBooking = _context.RentalBookings.FirstOrDefault(m =>
                    m.BookingID == id && (m.CustomerID == customerID || customerRole == "Admin"));

                if (rentalBooking == null)
                {
                    return NotFound("Booking not found or you do not have access to it.");
                }

                var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == customerID);

                if (customerID != null)
                {
                    var rent = _context.RentalBookings.Include(c => c.Customer).FirstOrDefault(c => c.CustomerID == customerID);
                    ViewBag.CutomerName = rent.Customer.CustomerName;
                }

                ViewData["VehicleID"] = new SelectList(_context.Vehicles, "VehicleID", "Model", rentalBooking.VehicleID);
                return View(rentalBooking);
            }

            return RedirectToAction("Login", "Customer");
        }

        // POST: RentalBookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RentalBooking rentalBooking)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (id != rentalBooking.BookingID)
                {
                    return NotFound();
                }

                BookingValidation(rentalBooking);
                CalculationDays(rentalBooking);

                var customerId = HttpContext.Session.GetInt32("CustomerID");
                rentalBooking.CustomerID = customerId.Value;

                if (!ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(rentalBooking);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RentalBookingExists(rentalBooking.BookingID))
                        {
                            return NotFound("Booking not found or you do not have access to it.");
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction("Details", new { id = rentalBooking.BookingID });
                }

                ViewData["VehicleID"] = new SelectList(_context.Vehicles, "VehicleID", "Model", rentalBooking.VehicleID);
                return View(rentalBooking);
            }

            return RedirectToAction("Login", "Customer");
        }

        // GET: RentalBookings/Delete/5
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                var customerID = HttpContext.Session.GetInt32("CustomerID");
                var customerRole = HttpContext.Session.GetString("Role");

                if (id == null)
                {
                    return NotFound();
                }

                var rentalBooking = _context.RentalBookings
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicles)
                    .FirstOrDefault(m => m.BookingID == id && (m.CustomerID == customerID || customerRole == "Admin"));

                if (rentalBooking == null)
                {
                    return NotFound("Booking not found or you do not have access to it.");
                }

                return View(rentalBooking);
            }

            return RedirectToAction("Login", "Customer");
        }

        // POST: RentalBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                var rentalBooking = _context.RentalBookings.Find(id);
                if (rentalBooking != null)
                {
                    _context.RentalBookings.Remove(rentalBooking);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Login", "Customer");
        }

        private bool RentalBookingExists(int id)
        {
            return _context.RentalBookings.Any(e => e.BookingID == id);
        }

        public IActionResult CheckAvailability(int vehicleId, DateTime startDate, DateTime endDate)
        {

            var Booking = _context.RentalBookings
            .FirstOrDefault(r => r.VehicleID == vehicleId &&
            ((r.RentalStartDate >= startDate && r.RentalStartDate <= endDate) ||
             (r.RentalEndDate >= startDate && r.RentalEndDate <= endDate) ||
             (r.RentalStartDate <= startDate && r.RentalEndDate >= endDate)));

   
            if (Booking != null)
            {
                var vehicle = _context.Vehicles.Find(vehicleId);
                if (vehicle.ISAvailabelForRent)
                {
                    return Json(new { isAvailable = true });
                }
                return Json(new
                {
                    isAvailable = false,
                    startDate = Booking.RentalStartDate.ToShortDateString(),
                    endDate = Booking.RentalEndDate.ToShortDateString()
                });
            }

            return Json(new { isAvailable = true });
        }


        public IActionResult GetVehicleDetails(int vehicleId)
        {
            var vehicle = _context.Vehicles.Include(r => r.FuleType).FirstOrDefault(v => v.VehicleID == vehicleId);
            var v = vehicle?.Year.Year;
            var t = vehicle?.TransmissionType?.Substring(0,1) + "T";
            var fulet = vehicle?.FuleType?.FuleType;
            if (vehicle != null)
            {
                return Json(new
                {
                    model = vehicle.Model,
                    year = v,
                    image = vehicle.Image,
                    fule = fulet,
                    price = vehicle.RentalPricePerDay,
                    transmission = t,
                    seat = vehicle.SeatsCount,
                    mile = vehicle.Mileage
                });
            }
            return Json(null);
        }




        private void BookingValidation(RentalBooking rentalBooking)
        {
            if (rentalBooking.RentalStartDate.Date <= DateTime.Now.Date)
            {
                ModelState.AddModelError("RentalStartDate", "Rental Start Date must be in the future.");
            }

            if (rentalBooking.InitialRentalDays < 1)
            {
                ModelState.AddModelError("InitialRentalDays", "Initial Rental Days must be more than one day.");
            }

            var overlappingBooking = _context.RentalBookings
                .Where(rb => rb.VehicleID == rentalBooking.VehicleID &&
                             rb.RentalStartDate < rentalBooking.RentalEndDate &&
                             rentalBooking.RentalStartDate < rb.RentalEndDate)
                .FirstOrDefault();

            if (overlappingBooking != null)
            {
                ModelState.AddModelError("RentalStartDate", $"This vehicle is already booked from {overlappingBooking.RentalStartDate.ToShortDateString()} to {overlappingBooking.RentalEndDate.ToShortDateString()}.");
            }
        }

        private void CalculationDays(RentalBooking rentalBooking)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleID == rentalBooking.VehicleID);
            rentalBooking.InitialRentalDays = (rentalBooking.RentalEndDate - rentalBooking.RentalStartDate).Days;
            rentalBooking.RentalPricePerDay = vehicle.RentalPricePerDay;
            rentalBooking.InitialTotalDueAmount = rentalBooking.RentalPricePerDay * rentalBooking.InitialRentalDays;
            vehicle.ISAvailabelForRent = false;
        }
    }
}
