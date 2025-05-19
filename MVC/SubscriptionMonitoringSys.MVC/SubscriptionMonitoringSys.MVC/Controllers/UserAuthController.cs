using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using SubscriptionMonitoringSys.MVC.Models;
using SubscriptionMonitoringSys.MVC.Services;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class UserAuthController : Controller
    {
        
            private readonly HttpClient _httpClient;
            private readonly IConfiguration _configuration;
        private readonly ProjectService _projectService;
            private string _WebBaseUrl;

            public UserAuthController(HttpClient httpClient, IConfiguration configuration, ProjectService projectService)
            {

            _httpClient = httpClient;
            _configuration = configuration;
            _WebBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _projectService = projectService;

            }


            public IActionResult Index()
            {
                return View();
            }

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(LoginModel model)
            {

                var response = await _httpClient.PostAsJsonAsync(_WebBaseUrl + "/Users/login", model);

                if (!response.IsSuccessStatusCode)
                {
                    return View(model);
                }

                var tokenResponse = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(tokenResponse);
                HttpContext.Session.SetString("JWToken", token.Token);
                HttpContext.Session.SetString("UserId", token.UserId);
                HttpContext.Session.SetString("Role", token.Role);
                return RedirectToAction("Index", "Home");
                }
            [HttpGet]
            public IActionResult CompanyUserRegister(string ProjectId)
            {
                var model = new RegisterModel
                {
                    UserId = ProjectId,
                    
                };
                 HttpContext.Session.SetString("ProjectId", model.UserId);

            return View(model);
            }
            [HttpPost]
            public async Task<IActionResult> CompanyUserRegister(RegisterModel model)
            {
                
            
                var response = await _httpClient.PostAsJsonAsync(_WebBaseUrl + "/Users/register", model);

                if (!response.IsSuccessStatusCode)
                {
                    
                        var newResponse = await _httpClient.DeleteAsync(_WebBaseUrl + $"/Projects/{model.UserId}");

                        if (!newResponse.IsSuccessStatusCode)
                        {
                             ViewBag.response = "no response";
                            return View();
                        }

                ViewBag.response = "UserId Not Created Successfully";
                return RedirectToAction("Index", "Home");
                   
                }

                return RedirectToAction("Index", "Project");
            }


            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(RegisterModel model)
            {
            if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrEmpty(model.UserId))
            {
                ViewBag.ValidationMessage = "UserID is required.";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrEmpty(model.UserName))
            {
                ViewBag.ValidationMessage = "Username is required.";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.UserPassword))
            {
                ViewBag.ValidationMessage = "Password is required.";
                return View(model);
            }
            if (model.UserPassword.Length < 6)
            {
                ViewBag.ValidationMessage = "Password must be at least 6 characters long.";
                return View(model);
            }
            
            string sessionRole = HttpContext.Session.GetString("Role");
           
                var response = await _httpClient.PostAsJsonAsync(_WebBaseUrl + "/Users/register", model);

                if (!response.IsSuccessStatusCode)
                {
                    return View(model);
                }

                // If validation passes
                return RedirectToAction("Login", "UserAuth");

            }
        
            public IActionResult Logout()
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }
        
    }
}
