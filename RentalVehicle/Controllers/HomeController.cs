using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalVehicle.Data;
using RentalVehicle.Models;

namespace RentalVehicle.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RentalDbContext _context;
        public HomeController(ILogger<HomeController> logger, RentalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var vehicles = _context.Vehicles.Include(v => v.FuleType).Include(v => v.VehicleCategories).OrderByDescending(v => v.VehicleID).Take(10);
            ViewBag.VehicleID = new SelectList(vehicles, "VehicleID", "Model");
            return View(vehicles.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View("Service");
        }
        public IActionResult Contact()
        {
            return View("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
