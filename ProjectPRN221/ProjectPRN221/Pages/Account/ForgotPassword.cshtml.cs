using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Helpers;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly NorthwindContext _db;
        public ForgotPasswordModel(NorthwindContext db)
        {
            _db = db;
        }
        [BindProperty]
        public string email { get; set; }
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
            if (String.IsNullOrEmpty(email))
            {
                TempData["msg"] = "Please enter your email to get password!";
                return Page();
            }

            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
            if (account == null)
            {
                TempData["msg"] = "Not found email, please check again!";
                return Page();
            }
            String password = account.Password;

            string bodyMail = "Your password is: " + password + "";

            var body = bodyMail;
            SendPasswordMail.SendMail(email, body);
            TempData["msg"] = "Please check you email!";
            return Page();

        }
    }
}
