using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Cookies;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public Models.Order Order { get; set; }
        private readonly NorthwindContext db;
        public CheckoutModel(NorthwindContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPostCheckout()
        {
            Models.Account account = Class.GetAccountFromSession(HttpContext.Session);
            Order.OrderDate = DateTime.Now;
            Order.CustomerId = account.CustomerId;
            var orderDetailsCard = Class.GetCartInfo(HttpContext);
            foreach (var item in orderDetailsCard)
            {
                Models.Product p = db.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == item.Value.ProductId);
                if (p == null)
                {
                    this.ViewData["Error"] = $"Product '{p.ProductName}' is not exist anymore";
                    return Page();
                }
                if (p.UnitsInStock < item.Value.Quantity)
                {
                    this.ViewData["Error"] = $"Product '{p.ProductName}' is out of stock";
                    return Page();
                }
                item.Value.Product = null;
            }
            Order.OrderDetails = orderDetailsCard.Select(item => item.Value).ToList();
            db.Orders.Add(Order);
            db.SaveChanges();
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize(new Dictionary<int, OrderDetail>()), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            return RedirectToPage("Index");
        }
    }
}
