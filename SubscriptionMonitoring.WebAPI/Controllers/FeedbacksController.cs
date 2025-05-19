using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.MVC.Data;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;
using SubscriptionMonitoring.WebAPI.Services;

namespace SubscriptionMonitoring.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FeedbackService _feedbackService;

        public FeedbacksController(AppDbContext context, FeedbackService feedbackService)
        {
            _context = context;
            _feedbackService = feedbackService;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        

        [HttpGet("user/{userid}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetUserFeedbacks(string userid)
        {
            return await _context.Feedbacks.Where(u => u.UserId == userid).ToListAsync();
        }

        [HttpGet("company/{companyid}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetCompanyFeedbacks(string companyid)
        {
            return await _context.Feedbacks.Where(u => u.UserId == companyid).ToListAsync();
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, Feedback feedback)
        {
            if (id != feedback.FeedbackId)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
            var newFeedback = await _feedbackService.CreateFeedbackAsync(feedback);

            return CreatedAtAction("GetFeedback", new { id = feedback.FeedbackId }, feedback);
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            await _feedbackService.DeleteFeedbackAsync(feedback);

            return NoContent();
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
