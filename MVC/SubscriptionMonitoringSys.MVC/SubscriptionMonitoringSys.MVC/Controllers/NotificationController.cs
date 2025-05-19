using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public NotificationController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<NotificationModel> notifications = new List<NotificationModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;


            if (SessionRole == "Admin")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Notifications");
                response = httpResponseMessage;
            }

            else
            {
                HttpResponseMessage httpResponseMessage1 = await _httpClient.GetAsync(_apiBaseUrl + $"/Notifications/user/{SessionuserId}");
                response = httpResponseMessage1;
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<NotificationModel>());
            }
            
            string apiResponse = await response.Content.ReadAsStringAsync();
            notifications = JsonSerializer.Deserialize<List<NotificationModel>>(apiResponse);
            

            return View(notifications);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(NotificationModel model)
        {
            model.NotificationSender = HttpContext.Session.GetString("Role");
            var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "/Notifications/", model);

            if (!response.IsSuccessStatusCode)
            {
                if(model.NotificationSender=="Admin")
                    ViewBag.Message = "UserId Not Found";
                else if (model.NotificationSender == "company")
                    ViewBag.Message = "Project Not Found";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }


    }
}
