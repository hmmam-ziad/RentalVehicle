using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentalVehicle.Data;
using RentalVehicle.Models;
using RentalVehicle.PasswordHash;

namespace RentalVehicle.Controllers
{

    public class CustomerController : Controller
    {
        private readonly RentalDbContext _context;

        public CustomerController(RentalDbContext _context)
        {
            this._context = _context;
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var customers = _context.Customers.ToList();
                    return View(customers);
                }
                return RedirectToAction("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if(_context.Customers.Any(c => c.DriverLicenseNumber == customer.DriverLicenseNumber))
            {
                ModelState.AddModelError("DriverLicenseNumber", "Driver License Number already exists");
            }
            if(_context.Customers.Any(c => c.Email == customer.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }
            if (ModelState.IsValid)
            {
                customer.Role = "User";
                customer.Password = PasswordHelper.HashPassword(customer.Password);
                _context.Customers.Add(customer);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                HttpContext.Session.SetString("Role", customer.Role);

                return RedirectToAction(nameof(Login));
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var hashPassword = PasswordHelper.HashPassword(customer.Password);
                var customerData = _context.Customers.Where(c => c.Email == customer.Email && c.Password == hashPassword).FirstOrDefault();
                if (customerData != null)
                {
                    HttpContext.Session.SetInt32("CustomerID", customerData.CustomerID);
                    HttpContext.Session.SetString("Role", customerData.Role);
                    HttpContext.Session.SetString("isLogIn", "true");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Email or Password");
                    return View(customer);
                }
            }
            return View(customer);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Edit()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null || HttpContext.Session.GetString("isLogIn") != "true")
            {
                return RedirectToAction(nameof(Login));
            }

            var customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer updatedCustomer)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null || HttpContext.Session.GetString("isLogIn") != "true")
            {
                return RedirectToAction(nameof(Login));
            }

            var customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            // Check for email uniqueness
            if (_context.Customers.Any(c => c.Email == updatedCustomer.Email && c.CustomerID != customerId))
            {
                ModelState.AddModelError("Email", "Email is already in use by another account.");
            }

            if (ModelState.IsValid)
            {
                // Update fields
                customer.CustomerName = updatedCustomer.CustomerName;
                customer.ContactInformation = updatedCustomer.ContactInformation;
                customer.DriverLicenseNumber = updatedCustomer.DriverLicenseNumber;
                customer.Email = updatedCustomer.Email;

                // Update password only if it's provided
                if (!string.IsNullOrEmpty(updatedCustomer.Password))
                {
                    customer.Password = PasswordHelper.HashPassword(updatedCustomer.Password);
                }

                _context.Update(customer);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(updatedCustomer);
        }

        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if(HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    var customer = _context.Customers.Find(id);
                    ViewBag.Role = new SelectList(new[] {"Admin" , "User"});
                    if (customer == null)
                    {
                        return NotFound();
                    }
                    return View(customer);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }
        [HttpPost]
        public IActionResult Details(int? id, string role)
        {
            if (HttpContext.Session.GetString("isLogIn") == "true")
            {
                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    ViewBag.Role = new SelectList(new[] { "Admin", "User" });
                    var customer = _context.Customers.Find(id);
                    if (customer == null)
                    {
                        return NotFound();
                    }
                    customer.Role = role;
                    _context.SaveChanges();
                    return View(customer);
                }
                return View("AccessDenied");
            }
            return RedirectToAction("Login", "Customer");
        }
    }
}
