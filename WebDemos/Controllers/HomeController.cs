using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebDemos.Models;

namespace WebDemos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly INotyfService _notyf;
        public HomeController(ILogger<HomeController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            _notyf.Error("Some Error Message");
            _notyf.Warning("Some Error Message");
            _notyf.Information("Information Notification - closes in 4 seconds.", 4);

           /* _notifyService.Custom("Custom Notification - closes in 5 seconds.", 5, "whitesmoke", "fa fa-gear");
            _notifyService.Custom("Custom Notification - closes in 5 seconds.", 10, "#B600FF", "fa fa-home");*/

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            _notyf.Error("Some Error Message");
            _notyf.Warning("Some Error Message");
            _notyf.Information("Information Notification - closes in 4 seconds.", 4);

            /* _notifyService.Custom("Custom Notification - closes in 5 seconds.", 5, "whitesmoke", "fa fa-gear");
             _notifyService.Custom("Custom Notification - closes in 5 seconds.", 10, "#B600FF", "fa fa-home");*/

            return View();
        }

        [HttpPost]
        public IActionResult Login(Login login )
        {
            if(login.UserName == "correct")
            {
                _notyf.Success(login.UserName);
            }
            else
            {
                _notyf.Error(login.UserName);
            }
           
         

            /* _notifyService.Custom("Custom Notification - closes in 5 seconds.", 5, "whitesmoke", "fa fa-gear");
             _notifyService.Custom("Custom Notification - closes in 5 seconds.", 10, "#B600FF", "fa fa-home");*/

            return View();
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