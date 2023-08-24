using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.EmployeesManager
{
    public class EmpEditModel : PageModel
    {
        private readonly NorthwindContext _context;
        public EmpEditModel(NorthwindContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Employee Employee { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Employee = await _context.Employees.FindAsync(id);
            if (Employee == null)
            {
                return NotFound();
            }
            else
            {
                return Page();

            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            _context.Employees.Update(Employee);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
