using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;
using System.Runtime.InteropServices;
using ProjectPRN221.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ProjectPRN221.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly NorthwindContext db;

        public CartModel(NorthwindContext db)
        {
            this.db = db;
        }

        public Customer Customer { get; set; }
        public Dictionary<int,OrderDetail> OrderDetails { get; set; }
        public async Task<IActionResult> OnGetAsync(int productId,bool isBuyNow)
        {
            Models.Account account = Class.GetAccountFromSession(HttpContext.Session);
            if(account == null )
            {
                Customer = new Customer();
            }
            else
            {
                Customer = db.Customers.Find(account.CustomerId);
            }
            if (productId == 0)
            {
                return RedirectToPage("Index");
            }
            OrderDetails = Class.GetCartInfo(HttpContext);
            Models.Product  productFromDB = db.Products.Find(productId);
            if (productFromDB == null)
            {
                return null;
            }
            if (!OrderDetails.ContainsKey(productId))
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Product = productFromDB,
                    ProductId = productId,
                    Quantity = 1,
                    UnitPrice = (decimal)productFromDB.UnitPrice
                };
                OrderDetails.Add(productId, orderDetail);
            }
            else
            {
                OrderDetail orderDetailFromCart = OrderDetails[productId];
                orderDetailFromCart.Quantity++;
            }
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize(OrderDetails), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            if (isBuyNow)
                return RedirectToPage("Index");
            else
            {
                return Redirect(Request.Headers.Referer);
            }

        }
    }
}
