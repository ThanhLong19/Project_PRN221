using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Admin.Order
{
    public class ListModel : PageModel
    {
        private readonly NorthwindContext _db;
        public ListModel(NorthwindContext db)
        {
            _db = db;
        }
        public IList<Models.Order> Order { get; set; } = default!;
        public List<Models.Order> orderList { get; set; }

        [BindProperty(SupportsGet = true, Name = "currentPage")]
        public int currentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? dateFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? dateTo { get; set; }
        public int totalPages { get; set; }

        public const int pageSize = 10;
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("admin") == null)
                return RedirectToPage("/Account/Login");
            else
            {
                if (currentPage < 1)
                {
                    currentPage = 1;
                }
                int totalOrders = getTotalOrders();
                totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);
                orderList = getAllOrders();
                return Page();
            }
        }
        private int getTotalOrders()
        {
            var list = from order in _db.Orders orderby order.OrderDate ascending select order;
            if (dateFrom == null || dateTo == null
               || (dateFrom == null && dateTo == null))
            {
                return list.Count();
            }

            return list.Where(o => DateTime.Compare(o.OrderDate.Value, dateFrom.Value) >= 0
                    && DateTime.Compare(o.OrderDate.Value, dateTo.Value) <= 0).ToList().Count();

        }

        private List<Models.Order> getAllOrders()
        {
            var list = from order in _db.Orders
                       .Include(e => e.Employee)
                       .Include(c => c.Customer)
                       orderby order.OrderDate ascending
                       select order;
            List<Models.Order> orders;
            if (dateFrom == null || dateTo == null
                || (dateFrom == null && dateTo == null))
            {
                orders = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                orders = list.Where(o => DateTime.Compare(o.OrderDate.Value, dateFrom.Value) >= 0
                    && DateTime.Compare(o.OrderDate.Value, dateTo.Value) <= 0)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize).ToList();
            }
            return orders;
        }
    }
}
