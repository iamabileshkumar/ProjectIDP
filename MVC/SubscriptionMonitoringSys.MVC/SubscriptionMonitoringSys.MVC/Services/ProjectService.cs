using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SubscriptionMonitoringSys.MVC.Models;

namespace SubscriptionMonitoringSys.MVC.Services
{
    public class ProjectService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _apiBaseUrl;

        public ProjectService(HttpClient httpClient,IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebBaseUrl");
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<string> Register(ProjectModel model)
        {
            model.UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl + "/Projects/register", model);
            response.EnsureSuccessStatusCode();
            var project = await response.Content.ReadFromJsonAsync<ProjectModel>();
            return project.ProjectId;
        }

        public async Task<int> NumberOfProjects(string id)
        {
            
            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + $"/Projects/company/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            string apiResponse = await response.Content.ReadAsStringAsync();
            var Projects = JsonSerializer.Deserialize<List<ProjectModel>>(apiResponse);

            return Projects.Count;
        }
    }
}
