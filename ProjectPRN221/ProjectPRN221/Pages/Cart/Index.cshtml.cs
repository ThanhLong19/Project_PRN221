using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Cookies;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly NorthwindContext db;
        private Dictionary<int, OrderDetail> orderDetailsCard;

        public IndexModel(NorthwindContext db)
        {
            this.db = db;

        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Models.Account account = Class.GetAccountFromSession(HttpContext.Session);
            if (account == null)
            {
                return RedirectToPage("Login");
            }
            else
            {
                Customer = db.Customers.Find(account.CustomerId);
            }
            orderDetailsCard = Class.GetCartInfo(HttpContext);

            ViewData["totalAmount"] = orderDetailsCard.Values.Sum(e => e.UnitPrice * e.Quantity);
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(orderDetailsCard));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(DateTime? requiredShipDate)
        {
            ModelState.Remove("Customer.CustomerId");
            if (ModelState.IsValid)
            {
                Dictionary<int, OrderDetail> cart = Class.GetCartInfo(HttpContext);
                if (cart.Count == 0)
                {
                    ViewData["error-message"] = "Cart empty.";
                    return Page();
                }
                if (requiredShipDate <= DateTime.Now)
                {
                    ViewData["error-message"] = "Ship date must be greater than today.";
                    return Page();
                }
                Models.Account account = Class.GetAccountFromSession(HttpContext.Session);
                string customerId = null;
                if (account == null)
                {
                    return RedirectToPage("Login");
                }
                if (account != null)
                {
                    customerId = account.CustomerId;
                }

                Order order = new Order
                {
                    OrderDate = DateTime.Now,
                    RequiredDate = requiredShipDate,
                    CustomerId = customerId,
                    ShipAddress = Customer.Address,
                    ShipName = Customer.ContactName,
                };

                order.Freight = cart.Values.Sum(e => e.UnitPrice * e.Quantity);

                await db.Orders.AddAsync(order);
                await db.SaveChangesAsync();

                Order newOrder = db.Orders.OrderByDescending(e => e.OrderId).Take(1).Include(e => e.Customer).ToList()[0];

                db.Products.Where(e => cart.Keys.Contains(e.ProductId)).ToList().ForEach(
                    e =>
                    {
                        if (e.UnitsOnOrder == null)
                        { e.UnitsOnOrder = cart[e.ProductId].Quantity; }
                        else
                        { e.UnitsOnOrder += cart[e.ProductId].Quantity; }
                    });

                // Remove all product detail from orderDetail in Card
                cart.Values.ToList().ForEach(e => { e.OrderId = newOrder.OrderId; e.Product = null; });

                await db.OrderDetails.AddRangeAsync(cart.Values);


                //orderDetail.ForEach(e =>
                //{
                //   Models.Product product =  dBContext.Products.Find(e.ProductId);
                //   product.UnitsInStock = product.UnitsInStock = e.Quantity;
                //   dBContext.Products.Update(product);
                //});

                ViewData["error-message"] = "Order success";
                HttpContext.Response.Cookies.Delete("Cart");
                await db.SaveChangesAsync();

            }

            return Page();
        }



    }
}
