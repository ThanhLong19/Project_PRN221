using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;


namespace ProjectPRN221.Pages.EmployeesManager
{
    public class IndexModel : PageModel
    {
        private readonly NorthwindContext _context;
        [BindProperty]
        public List<Employee> ListEmployee { get; set; }
        public IndexModel(NorthwindContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            ListEmployee = await _context.Employees.ToListAsync();
             
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
