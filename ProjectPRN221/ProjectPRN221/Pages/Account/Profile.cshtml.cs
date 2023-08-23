using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly NorthwindContext _db;
        [BindProperty]
        public Models.Account account { get; set; }
        public ProfileModel(NorthwindContext db) 
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            account = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("customer"));

            if (account == null)
            {
                return NotFound();
            }
            else
            {
                account.Customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == account.CustomerId);
            }

            return Page();
        }
    }
}
