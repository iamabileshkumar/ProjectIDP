using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public SubscriptionController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _httpClient = httpClient;

        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<SubscriptionModel> subscriptions = new List<SubscriptionModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;


            if (SessionRole == "Admin")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions");
                response = httpResponseMessage;
            }
            else if (SessionRole == "company")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions/company/{SessionuserId}");
                response = httpResponseMessage;
            }
            else
            {
                HttpResponseMessage httpResponseMessage1 = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions/user/{SessionuserId}");
                response = httpResponseMessage1;
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<SubscriptionModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            subscriptions = JsonSerializer.Deserialize<List<SubscriptionModel>>(apiResponse);

            return View(subscriptions);
        }

        public async Task<IActionResult> ProjectSubscriptions(string id)
        {
            IEnumerable<SubscriptionModel> subscriptions = new List<SubscriptionModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions/user/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<SubscriptionModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            subscriptions = JsonSerializer.Deserialize<List<SubscriptionModel>>(apiResponse);

            return View(subscriptions);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SubscriptionModel model)
        {
            model.UserId = HttpContext.Session.GetString("UserId");
            
            var newResponse = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "/Subscriptions", model);

            if (!newResponse.IsSuccessStatusCode)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Subscription");
        }

        [HttpGet]
        public IActionResult RegisterByCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterByCompany(SubscriptionModel model)
        {
            

            var newResponse = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "/Subscriptions/company", model);

            if (!newResponse.IsSuccessStatusCode)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Subscription");
        }


        public async Task<IActionResult> Delete(int id)
        {
         
            var newResponse = await _httpClient.DeleteAsync(_apiBaseUrl + $"/Subscriptions/{id}");

            if (!newResponse.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index", "Subscription");
        }

        public async Task<IActionResult> Details(int id)
        {
            SubscriptionModel subscriptions = new SubscriptionModel();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(SessionRole))
            {
                // Handle the case where the session value is not set
                return View("Error", new ErrorViewModel { RequestId = $"Session user ID is not set in details.{id}" });
            }

            

            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = "Session user ID response is not set." });
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            subscriptions = System.Text.Json.JsonSerializer.Deserialize<SubscriptionModel>(apiResponse);

            return View(subscriptions);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View();
            }
            string apiResponse = await response.Content.ReadAsStringAsync();
             var subscriptions = System.Text.Json.JsonSerializer.Deserialize<SubscriptionModel>(apiResponse);

            return View(subscriptions);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id,SubscriptionModel model)
        {


            var options = new JsonSerializerOptions
            {
                
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                
            };

            var json = JsonSerializer.Serialize(model,options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var newResponse = await _httpClient.PutAsync(_apiBaseUrl + $"/Subscriptions/{id}",content);

            if (!newResponse.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index");
                
        }
        [HttpGet]
        public async Task<IActionResult> AllSubscriptionsInCompany(string id)
        {
            IEnumerable<SubscriptionModel> subscriptions = new List<SubscriptionModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Subscriptions/company/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<SubscriptionModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            subscriptions = JsonSerializer.Deserialize<List<SubscriptionModel>>(apiResponse);

            return View(subscriptions);
        }
    }
}
