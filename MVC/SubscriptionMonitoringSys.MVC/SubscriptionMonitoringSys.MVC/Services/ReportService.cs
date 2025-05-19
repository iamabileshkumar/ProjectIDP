using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;

namespace SubscriptionMonitoringSys.MVC.Services
{

    public class ReportService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IConfiguration configuration, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");

            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CombinedViewModel> GenerateReport(CombinedViewModel model)
        {
            
            var sessionRole = _httpContextAccessor.HttpContext.Session.GetString("Role");
            HttpResponseMessage response = null;
            IEnumerable<SubscriptionModel> report = new List<SubscriptionModel>();

            if (sessionRole == "User")
            {
                model.ReportFilter.UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                var newResponse = await _httpClient.PostAsJsonAsync(_apiBaseUrl + $"/Report/user/{model.ReportFilter.UserId}", model.ReportFilter);
                response = newResponse;
            }
            else if (sessionRole == "Admin" && !string.IsNullOrEmpty(model.ReportFilter.UserId))
            {
                var newResponse = await _httpClient.PostAsJsonAsync(_apiBaseUrl + $"/Report/user/{model.ReportFilter.UserId}", model.ReportFilter);
                response = newResponse;
            }
            
            else if (sessionRole == "company")
            {
                var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                Console.WriteLine(model.ReportFilter.UserId);
                if (!string.IsNullOrEmpty(model.ReportFilter.UserId))
                {
                    var newResponse = await _httpClient.PostAsJsonAsync(_apiBaseUrl + $"/Report/company/user/{model.ReportFilter.UserId}", model.ReportFilter);
                    response = newResponse;
                }
                else
                {
                    // If no UserId is provided, get all subscriptions data
                    var newResponse = await _httpClient.PostAsJsonAsync(_apiBaseUrl + $"/Report/company/{userId}", model.ReportFilter);
                    response = newResponse;
                }
            }
            else if(sessionRole == "Admin")
            {
                response = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "/Report/all", model.ReportFilter);
            }




            if (!response.IsSuccessStatusCode)
            {
                return new CombinedViewModel
                {
                    ReportFilter = model.ReportFilter,
                    Subscriptions = new List<SubscriptionModel>()
                };
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            report = JsonSerializer.Deserialize<List<SubscriptionModel>>(apiResponse);
            model.Subscriptions = report;
            return model;

        }

        public async Task<int> NumberOfUsers()
        {

            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "/Users");
            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            var Users = JsonSerializer.Deserialize<List<UserModel>>(apiResponse);
            Console.WriteLine(Users.Count);
            return Users.Count;
        }
    }
}
