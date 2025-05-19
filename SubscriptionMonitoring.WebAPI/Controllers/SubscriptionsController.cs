using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;
using SubscriptionMonitoring.WebAPI.Services;

namespace SubscriptionMonitoring.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SubscriptionService _subscriptionService;
        private readonly CategoryService _categoryService;
        private readonly ProjectService _projectService;
        private readonly NotificationService _notificationService;

        public SubscriptionsController(AppDbContext context, SubscriptionService subscriptionService, CategoryService categoryService, ProjectService projectService, NotificationService notificationService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
            _categoryService = categoryService;
            _projectService = projectService;
            _notificationService = notificationService;
        }

        // GET: api/Subscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        // GET: api/Subscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscriptionDetails(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            Console.WriteLine(Convert.ToString(id)+" "+ subscription.SubscriptionId);
            if (id != subscription.SubscriptionId)
            {
                return BadRequest();
            }

            int categoryId = _categoryService.CategoryExists(subscription.CategoryName);
            if (categoryId == 0)
            {
                var category = await _categoryService.GetOrCreateCategoryAsync(subscription.CategoryName);


                subscription.CategoryId = category.CategoryId;

            }
            else
                subscription.CategoryId = categoryId;

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(subscription);
        }

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
            var isUserExists = _subscriptionService.UserExists(subscription.UserId);
            if (isUserExists)
            {
                Subscription newSubscription = await _subscriptionService.CreateSubscriptionAsync(subscription);

                return CreatedAtAction("GetSubscription", new { id = newSubscription.SubscriptionId }, newSubscription);
            }
            return NotFound();
        }

        [HttpPost("company")]
        public async Task<ActionResult<Subscription>> CompanyPostSubscription(Subscription subscription)
        {
            var isUserExists = _subscriptionService.UserExists(subscription.UserId);
            var isProjectExists = _projectService.ProjectExists(subscription.UserId);
            Console.WriteLine(isProjectExists + " " + isUserExists+subscription.UserId);
            if (isUserExists && isProjectExists)
            {
                Subscription newSubscription = await _subscriptionService.CreateSubscriptionAsync(subscription);

                return CreatedAtAction("GetSubscription", new { id = newSubscription.SubscriptionId }, newSubscription);
            }
            return NotFound();
        }

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            var notification = new Notification()
            {
                NotificationSender = "Admin",
                NotificationMessage = $"{subscription.SubscriptionName} Deleted!",
                NotificationSentDate = DateTime.Today,
                UserId = subscription.UserId,
                Type = "Subscription Deleted"
            };
            Notification sendNotification = await _notificationService.CreateNotificationAsync(notification);

            return Ok();
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.SubscriptionId == id);
        }

        [HttpGet("user/{userid}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetUserSubscription(string userid)
        {
            return await _context.Subscriptions.Where(u => u.UserId == userid).ToListAsync();
        }

        [HttpGet("company/{companyid}")] /// should be modified to get in all projects 
        public async Task<ActionResult<IEnumerable<Subscription>>> GetCompanySubcription(string companyid)
        {
            List<Project> AllProjects = new List<Project>();
            AllProjects = _projectService.GetAllProjects(companyid);
            List<Subscription> AllSubscriptions = new List<Subscription>();
            foreach(var project in AllProjects)
            {
                var subscriptions = await _context.Subscriptions
                                           .Where(u => u.UserId == project.ProjectId)
                                           .ToListAsync();
                AllSubscriptions.AddRange(subscriptions);
            }
            if (AllSubscriptions.Count>0)
            {
                return Ok(AllSubscriptions);
            }
            return NotFound();
            

        }

    }
}
