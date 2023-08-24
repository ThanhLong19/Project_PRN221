using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly NorthwindContext _context;
        [BindProperty]
        public List<Models.Order> Orders { get; set; } 
        public IndexModel(NorthwindContext context)
        {
            _context = context;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public async Task<IActionResult> OnGetAsync(int? customerId, int? pageNumber)
        {
            PageSize = 4;
            PageIndex = pageNumber ?? 1;

            var acc = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("customer"));
            IQueryable<Models.Order> query = _context.Orders.Include(o => o.Employee).Where(o => o.CustomerId.Equals(acc.CustomerId));

            TotalItems = await query.CountAsync();

            Orders = await query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            return Page();
        }
    }
}
