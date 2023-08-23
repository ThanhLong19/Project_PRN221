using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Text.Json;

namespace ProjectPRN221.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly NorthwindContext _db;
        public LoginModel(NorthwindContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Models.Account Account { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("customer") != null || HttpContext.Session.GetString("admin") != null)
            {
                return RedirectToPage("Register");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var member = await _db.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Account.Email) && a.Password.Equals(Account.Password));
            if (member != null)
            {
                if (member.Role == 1)
                {
                    HttpContext.Session.SetString("admin", JsonSerializer.Serialize(member));
                    return RedirectToPage("/Index");
                }
                if (member.Role == 2)
                {
                    HttpContext.Session.SetString("customer", JsonSerializer.Serialize(member));
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                TempData["msg"] = "Email or Password invalid";
                return Page();
            }
            return Page();
        }
    }
}
