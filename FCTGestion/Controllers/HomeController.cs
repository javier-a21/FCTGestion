using System.Diagnostics;
using FCTGestion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;

namespace FCTGestion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [AllowAnonymous]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult LegalNotice()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult CookiesPolicy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult GenerarPDF()
        {
            return new ViewAsPdf("DemoPDF")
            {
                FileName = "ejemplo.pdf"
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
