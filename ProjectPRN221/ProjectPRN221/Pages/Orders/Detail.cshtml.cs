using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Orders
{
    public class DetailModel : PageModel
    {
        private readonly NorthwindContext _context;
        public List<OrderDetail> OrderDetails { get; set; }
        public DetailModel(NorthwindContext context)
        {
            _context = context;
        }
        [BindProperty]
        public decimal TotalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? orderId)
        {
            var acc = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("customer"));
            OrderDetails = await _context.OrderDetails.Include(od => od.Product).Include(od => od.Order).Where(od => od.OrderId==orderId && od.Order.CustomerId.Equals(acc.CustomerId)).ToListAsync();
            foreach(var od in OrderDetails)
            {
                TotalPrice += od.Quantity * od.UnitPrice;
            }
            return Page();
        }
    }
}
