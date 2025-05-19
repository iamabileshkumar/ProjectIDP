using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using SubscriptionMonitoringSys.MVC.Models;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public FeedbackController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _httpClient = httpClient;

        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<FeedbackModel> feedbacks = new List<FeedbackModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;


            if (SessionRole == "Admin")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Feedbacks");
                 response = httpResponseMessage;
            }
            else
            {
                HttpResponseMessage httpResponseMessage1 = await _httpClient.GetAsync(_apiBaseUrl + $"/Feedbacks/user/{SessionuserId}");
                 response = httpResponseMessage1;
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<FeedbackModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            feedbacks = JsonSerializer.Deserialize<List<FeedbackModel>>(apiResponse);

            return View(feedbacks);
        }
        [HttpGet]
        public async Task<IActionResult> ProjectFeedbacks(string id)
        
        {
            IEnumerable<FeedbackModel> feedbacks = new List<FeedbackModel>();
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Feedbacks/user/{id}");


            if (!response.IsSuccessStatusCode)
            {
                return View(new List<FeedbackModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            feedbacks = JsonSerializer.Deserialize<List<FeedbackModel>>(apiResponse);

            return View(feedbacks);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(FeedbackModel model)
        {
            model.UserId = HttpContext.Session.GetString("UserId");
            var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "/Feedbacks/register", model);

            Console.WriteLine(model.UserId + " "+model.FeedbackRating+" "+model.FeedbackText+" "+model.FeedbackId);


            if (!response.IsSuccessStatusCode)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Feedback");
        }

        public async Task<IActionResult> Details(int id)
        {
            FeedbackModel feedbacks = new FeedbackModel();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(SessionRole))
            {
                // Handle the case where the session value is not set
                return View("Error", new ErrorViewModel { RequestId = $"Session user ID is not set in details.{id}" });
            }



            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Feedbacks/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = "Session user ID response is not set." });
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            feedbacks = System.Text.Json.JsonSerializer.Deserialize<FeedbackModel>(apiResponse);

            return View(feedbacks);
        }

    }
}
