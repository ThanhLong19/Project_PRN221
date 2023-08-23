using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPRN221.Pages.Product
{
    public class ListModel : PageModel
    {
        private readonly NorthwindContext _context;
        [BindProperty]
        public List<Category> Categories { get; set; }
        [BindProperty]
        public List<Models.Product> Products { get; set; }
        public ListModel(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string? nameSearch, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            Categories = await _context.Categories.ToListAsync();
            IQueryable<Models.Product> query = _context.Products.Where(p => p.UnitsInStock > 0);
            if (nameSearch != null)
            {
                query = query.Where(p => p.ProductName.Contains(nameSearch));
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                query = query.Where(p => (p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice));
            }

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                query = query.Where(p => p.Category.CategoryId == categoryId);
            }
            Products = await query.ToListAsync();
            return Page();
        }
    }
}
