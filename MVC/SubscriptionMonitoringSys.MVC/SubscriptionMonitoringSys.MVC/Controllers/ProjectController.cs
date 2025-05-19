using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;
using SubscriptionMonitoringSys.MVC.Services;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ProjectService _projectService;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ProjectController(IConfiguration configuration, HttpClient httpClient,ProjectService projectService)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _httpClient = httpClient;
            _projectService = projectService;

        }
        public async Task<IActionResult> Index()
        {

            IEnumerable<ProjectModel> Projects = new List<ProjectModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;


            if (SessionRole == "Admin")
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_apiBaseUrl + $"/Projects");
                response = httpResponseMessage;
            }
            else
            {
                HttpResponseMessage httpResponseMessage1 = await _httpClient.GetAsync(_apiBaseUrl + $"/Projects/company/{SessionuserId}");
                response = httpResponseMessage1;
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ProjectModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            Projects = JsonSerializer.Deserialize<List<ProjectModel>>(apiResponse);

            return View(Projects);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ProjectModel model)
        {
            string sessionRole = HttpContext.Session.GetString("Role");
            if(sessionRole=="Company")
            if (model == null)
            {
                return BadRequest();
            }

            // Create the project
            string projectId = await _projectService.Register(model);
            HttpContext.Session.SetString("ProjectId", projectId);
            // Redirect to the user registration page with the project details
            return RedirectToAction("CompanyUserRegister", "UserAuth", new { projectId = projectId });

        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            ProjectModel projects = new ProjectModel();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(SessionRole))
            {
                // Handle the case where the session value is not set
                return View("Error", new ErrorViewModel { RequestId = $"Session user ID is not set in details.{id}" });
            }



            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Projects/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel { RequestId = "Session user ID response is not set." });
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            projects = System.Text.Json.JsonSerializer.Deserialize<ProjectModel>(apiResponse);

            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Projects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View();
            }
            string apiResponse = await response.Content.ReadAsStringAsync();
            var projects = System.Text.Json.JsonSerializer.Deserialize<ProjectModel>(apiResponse);

            return View(projects);
        }

        [HttpPost]

        public async Task<IActionResult> Update(string id, ProjectModel model)
        {


            var options = new JsonSerializerOptions
            {

                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

            };

            var json = JsonSerializer.Serialize(model, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var newResponse = await _httpClient.PutAsync(_apiBaseUrl + $"/Projects/{id}", content);

            if (!newResponse.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {

            var newResponse = await _httpClient.DeleteAsync(_apiBaseUrl + $"/Projects/{id}");

            if (!newResponse.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AllProjectsInCompany(string id)
        {

            IEnumerable<ProjectModel> Projects = new List<ProjectModel>();
            string SessionuserId = HttpContext.Session.GetString("UserId");
            string SessionRole = HttpContext.Session.GetString("Role");
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Projects/company/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ProjectModel>());
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            Projects = JsonSerializer.Deserialize<List<ProjectModel>>(apiResponse);

            return View(Projects);
        }
    }
}
