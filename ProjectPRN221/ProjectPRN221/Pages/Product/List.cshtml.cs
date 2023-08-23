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
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public async Task<IActionResult> OnGetAsync(string? nameSearch, decimal? minPrice, decimal? maxPrice, int? categoryId, int? pageNumber)
        {
            Categories = await _context.Categories.ToListAsync();

            PageSize = 8;
            PageIndex = pageNumber ?? 1;
            IQueryable<Models.Product> query = _context.Products.Where(p => p.UnitsInStock > 0);

            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(p => p.ProductName.Contains(nameSearch));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.UnitPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.UnitPrice <= maxPrice.Value);
            }

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                query = query.Where(p => p.Category.CategoryId == categoryId.Value);
            }

            TotalItems = await query.CountAsync();

            Products = await query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }
    }
}
