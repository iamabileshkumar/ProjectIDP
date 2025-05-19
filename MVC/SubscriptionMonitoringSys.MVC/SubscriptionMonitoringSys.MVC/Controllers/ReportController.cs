using System.Composition;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;
using SubscriptionMonitoringSys.MVC.Services;

namespace SubscriptionMonitoringSys.MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly ReportService _reportService;

        public ReportController(IConfiguration configuration, HttpClient httpClient,ReportService reportService)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _reportService = reportService;
            _httpClient = httpClient;

        }


        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new CombinedViewModel
            {
                ReportFilter = new ReportModel(),
                Subscriptions = Enumerable.Empty<SubscriptionModel>()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CombinedViewModel model)
        {
            var newModel = await _reportService.GenerateReport(model);
            return View(newModel);

        }
    }
}
