using Microsoft.AspNetCore.Mvc;
using SuccessPointCore.Domain.Entities;
using System.Diagnostics;
using System.Reflection;

namespace SuccessPointCore.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewData["Environment"] = _env.IsDevelopment() ? "Development" : "Production";

            ViewData["Version"] = GetFileVersion();

            return View();
        }

        private static string? GetFileVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var FileVersion = fileVersionInfo.FileVersion;
            return FileVersion;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
