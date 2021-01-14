using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using web.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace web.Controllers
{
    public class HelloController : Controller
    {
        private readonly ILogger<HelloController> _logger;
        private readonly IConfiguration _configuration;
        private string apiBaseUrl;
        public HelloController(ILogger<HelloController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            apiBaseUrl = _configuration.GetValue<string>("APIBaseUrl");
        }
        public IActionResult Index()
        {
            var hello = "Can't reach the API server";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                try
                {
                    var response = client.GetAsync("Hello?name=Alex").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        hello = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        hello = "The API server returned an error";
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                catch (Exception)
                {
                    hello = "Can't reach the API server";
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            ViewData["Hello"] = hello;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
