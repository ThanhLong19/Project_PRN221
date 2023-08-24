using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.ChartControl
{
    public class ChartModel : PageModel
    {
        private readonly NorthwindContext _context;
        public ChartModel(NorthwindContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<string> Months { get; set; }
        [BindProperty]
        public List<float> Revenue { get; set; }
        [BindProperty]
        public List<int> AvailableYears { get; set; }


        public IActionResult OnGetAsync(int selectedYear)
        {
           
            Months = new List<string>();
            Revenue = new List<float>();
            AvailableYears = _context.Orders.Select(order => order.OrderDate.Year).Distinct().ToList();

            if (selectedYear < 1 || selectedYear > 9999)
            {
                var years = _context.Orders.Select(x => x.OrderDate.Year).Max();

                selectedYear = years;
            }
            Console.WriteLine(selectedYear);
            //Months = new List<string>();
            for (int month = 1; month <= 12; month++)
            {
                int daysInMonth = DateTime.DaysInMonth(selectedYear, month);
                DateTime firstDayOfMonth = new DateTime(selectedYear, month, 1);
                string monthName = firstDayOfMonth.ToString("MMMM");
                Months.Add(monthName);
            }

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Year == selectedYear)
                .ToList();

            //Revenue = new List<float>();

            foreach (var month in Enumerable.Range(1, 12))
            {
                var totalRevenue = orders
                    .Where(order => order.OrderDate.Month == month)
                    .Sum(order => order.OrderDetails.Sum(detail => ((float)detail.UnitPrice * (float)detail.Quantity * (float)(1 - detail.Discount)))); // Cast to decimal

                Revenue.Add(totalRevenue);
            }

                var result = new { Months = Months, Revenue = Revenue };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // If it's an AJAX request, return JSON data
                return new JsonResult(result);
            }
            else
            {
                // Otherwise, render the page
                return Page();
            }
           
        }



    }
}
