using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;
using SubscriptionMonitoringSys.MVC.Services;

namespace SubscriptionMonitoringSys.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;
    private readonly ReportService _reportService;
    private readonly ProjectService _projectService;



    public HomeController(IConfiguration configuration, HttpClient httpClient, ReportService reportService, ProjectService projectService)
    {
        _configuration = configuration;
        _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
        _reportService = reportService;
        _httpClient = httpClient;
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var sessionRole = HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userId))
        {
            // Handle the case where UserId is null, e.g., redirect to login page
            return RedirectToAction("Login", "Account");
        }
        var response = await _httpClient.GetAsync(_apiBaseUrl + $"/Notifications/top3/{userId}");
        if (!response.IsSuccessStatusCode)
        {
            return View();
        }
        string apiResponse = await response.Content.ReadAsStringAsync();
        var notifications = JsonSerializer.Deserialize<List<NotificationModel>>(apiResponse);
        var reportFilter = new ReportModel { UserId = (sessionRole == "company" || sessionRole == "Admin") ? null : userId };
        
        var numberOfProjects = await _projectService.NumberOfProjects(userId);
        var numberOfUsers = await _reportService.NumberOfUsers();

        var newModel = new CombinedViewModel(reportFilter, notifications, numberOfProjects,numberOfUsers);

        var newModelResponse = await _reportService.GenerateReport(newModel);

        if (newModelResponse == null || newModelResponse.Subscriptions == null)
        {
            newModelResponse = new CombinedViewModel
            {
                ReportFilter = newModel.ReportFilter,
                Subscriptions = new List<SubscriptionModel>()

            };
        }
        ViewBag.TotalUsers = newModelResponse.NumberOfUsers;
        ViewBag.TotalProjects = newModelResponse.NumberOfProjects;
        ViewBag.price = 0;
        ViewBag.numberOfActiveSubscriptions = 0;
        ViewBag.numberOfExpiresSoonSubscriptions = 0;
        ViewBag.numberOfExpiredSubscriptions = 0;
        ViewBag.numberOfCancelledSubscriptions = 0;
        
        foreach (var item in newModelResponse.Subscriptions)
        {
            Console.WriteLine(item);
            ViewBag.price += item.SubscriptionPrice;
            
            if (item.Status == "Active")
            {
                ViewBag.numberOfActiveSubscriptions += 1;
            }
            else if (item.Status == "Expired Soon")
            {
                ViewBag.numberOfExpiresSoonSubscriptions += 1;
            }
            else if (item.Status == "Expired")
            {
                ViewBag.numberOfExpiredSubscriptions += 1;
            }
            else
            {
                ViewBag.numberOfCancelledSubscriptions += 1;
            }
        }
        return View(newModelResponse);
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
