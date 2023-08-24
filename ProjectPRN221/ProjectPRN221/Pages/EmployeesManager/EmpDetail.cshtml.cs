using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.EmployeesManager
{
    public class EmpDetailModel : PageModel
    {
        private readonly NorthwindContext _context;
        [BindProperty]
        public Employee employee { get; set; }
        public EmpDetailModel(NorthwindContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            employee = _context.Employees.FirstOrDefault(x => x.EmployeeId == id);
            return Page();
        }
    }
}
