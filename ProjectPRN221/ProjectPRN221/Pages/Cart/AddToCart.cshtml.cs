using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Cookies;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Cart
{
    public class AddToCartModel : PageModel
    {
        private readonly NorthwindContext db;

        public AddToCartModel(NorthwindContext db)
        {
            this.db = db;
        }

        public Customer Customer { get; set; }
        public Dictionary<int, OrderDetail> orderDetailsCard;

        public IActionResult OnGet(int productId, bool isBuyNow)
        {
            Models.Account account = Class.GetAccountFromSession(HttpContext.Session);
            if (account == null)
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
            orderDetailsCard = Class.GetCartInfo(HttpContext);
            Models.Product productFromDB = db.Products.Find(productId);
            if (productFromDB == null)
            {
                return null;
            }
            if (orderDetailsCard == null)
            {
                orderDetailsCard = new Dictionary<int, OrderDetail>();
            }
            if (!orderDetailsCard.ContainsKey(productId))
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    Product = productFromDB,
                    ProductId = productId,
                    Quantity = 1,
                    UnitPrice = (decimal)productFromDB.UnitPrice
                };
                orderDetailsCard.Add(productId, orderDetail);
            }
            else
            {
                orderDetailsCard[productId].Quantity++;
            }
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize(orderDetailsCard), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            //if (isBuyNow)
            //    return RedirectToPage("Index");
            //else
            //{
            //    return Redirect(Request.Headers.Referer);
            //}
            return RedirectToPage("/Cart/Index");

        }
    }
}
