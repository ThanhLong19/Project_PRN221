using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Admin.Order
{
    public class DetailModel : PageModel
    {
        private readonly NorthwindContext _db;
        public DetailModel(NorthwindContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Models.Order order { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("admin") == null)
                return RedirectToPage("/Account/Login");
            else
            {
                if (id == null)
                {
                    return RedirectToPage("/Index");
                }
                order = await _db.Orders.Include(o => o.Employee).Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == id);
                order.OrderDetails = (from od in _db.OrderDetails select od)
                    .Where(o => o.OrderId == order.OrderId)
                    .ToList();
                foreach (var o in order.OrderDetails)
                {
                    o.Product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == o.ProductId);
                }
                return Page();
            }
        }
    }
}
