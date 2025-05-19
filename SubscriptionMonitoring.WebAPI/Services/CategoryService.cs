using Microsoft.EntityFrameworkCore;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;

namespace SubscriptionMonitoring.WebAPI.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Category> GetOrCreateCategoryAsync(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
            if (category == null)
            {
                category = new Category { CategoryName = categoryName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            return category;
        }

        public int CategoryExists(string categoryName)
        {
            var category =  _context.Categories.FirstOrDefault(e => e.CategoryName == categoryName);
            if (category != null)
                return category.CategoryId;
            else
                return 0;

        }

        
    }
}
