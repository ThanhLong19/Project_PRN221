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
            if (HttpContext.Session.GetString("customer") == null)
            {
                return Redirect("/Account/Login");
            }
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
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Models.Account auth = JsonSerializer.Deserialize<Models.Account>(HttpContext.Session.GetString("customer"));

                var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountId == auth.AccountId);

                if (acc.CustomerId != null)
                
                    acc.Customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == acc.CustomerId);
                

                //acc.Email = Auth.Email;
                acc.Customer.Phone = account.Customer.Phone;
                acc.Customer.CompanyName = account.Customer.CompanyName;
                acc.Customer.ContactName = account.Customer.ContactName;
                acc.Customer.ContactTitle = account.Customer.ContactTitle;
                acc.Customer.Address = account.Customer.Address;

                await _db.SaveChangesAsync();
                ViewData["success"] = "Update Successfull";
                return Page();
            }
            catch (Exception e)
            {
                ViewData["fail"] = e.Message;
                return Page();
            }

        }
    }
}
