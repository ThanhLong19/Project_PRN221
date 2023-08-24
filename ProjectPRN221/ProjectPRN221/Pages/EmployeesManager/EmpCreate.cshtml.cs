using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.EmployeesManager
{
    public class EmpCreateModel : PageModel
    {
        private readonly NorthwindContext _context;
        [BindProperty]
        public Employee employee { get; set; }

       
        public EmpCreateModel(NorthwindContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
           
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
