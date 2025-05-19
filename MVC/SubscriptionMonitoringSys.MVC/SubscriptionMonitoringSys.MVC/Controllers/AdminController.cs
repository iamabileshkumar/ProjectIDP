using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public AdminController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserModel> Users = new List<UserModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;


            if (SessionRole == "Admin")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Users");
                response = httpResponseMessage;
            }
            else
            {
                HttpResponseMessage httpResponseMessage1 = await _httpClient.GetAsync(_apiBaseUrl + $"/Users/{SessionuserId}");
                response = httpResponseMessage1;
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<UserModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            Users = JsonSerializer.Deserialize<List<UserModel>>(apiResponse);

            return View(Users);
        }
        
        public async Task<IActionResult> Companies()
        {
            IEnumerable<UserModel> Users = new List<UserModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;


            if (SessionRole == "Admin")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Users");
                response = httpResponseMessage;
            }
            else
            {
                HttpResponseMessage httpResponseMessage1 = await _httpClient.GetAsync(_apiBaseUrl + $"/Users/{SessionuserId}");
                response = httpResponseMessage1;
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<UserModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            Users = JsonSerializer.Deserialize<List<UserModel>>(apiResponse);

            return View(Users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            UserModel users = new UserModel();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(SessionRole))
            {
                // Handle the case where the session value is not set
                return View("Error", new ErrorViewModel { RequestId = $"Session user ID is not set in details.{id}" });
            }



            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = "Session user ID response is not set." });
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            users = System.Text.Json.JsonSerializer.Deserialize<UserModel>(apiResponse);

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> CompanyDetails(string id)
        {
            UserModel users = new UserModel();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(SessionRole))
            {
                // Handle the case where the session value is not set
                return View("Error", new ErrorViewModel { RequestId = $"Session user ID is not set in details.{id}" });
            }



            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = "Session user ID response is not set." });
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            users = System.Text.Json.JsonSerializer.Deserialize<UserModel>(apiResponse);

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Users/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View();
            }
            string apiResponse = await response.Content.ReadAsStringAsync();
            var users = System.Text.Json.JsonSerializer.Deserialize<UserModel>(apiResponse);

            return View(users);
        }

        [HttpPost]

        public async Task<IActionResult> Update(string id, UserModel model)
        {


            var options = new JsonSerializerOptions
            {

                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

            };

            var json = JsonSerializer.Serialize(model, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var newResponse = await _httpClient.PutAsync(_apiBaseUrl + $"/Users/{id}", content);

            if (!newResponse.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {

            var newResponse = await _httpClient.DeleteAsync(_apiBaseUrl + $"/Users/{id}");

            if (!newResponse.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index");
        }


    }
}
