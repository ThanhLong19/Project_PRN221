using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Cookies;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Cart
{
    public class RemoveCartModel : PageModel
    {
        public Dictionary<int, OrderDetail> orderDetails { get; set; }

        public IActionResult OnGet(int productId)
        {
            orderDetails = Class.GetCartInfo(HttpContext);

            if (orderDetails.ContainsKey(productId))
            {
                orderDetails.Remove(productId);
            }
            HttpContext.Response.Cookies.Append("Cart", JsonSerializer.Serialize(orderDetails), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            return RedirectToPage("Index");
        }
    }
}
