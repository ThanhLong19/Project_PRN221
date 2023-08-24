using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("customer");
            HttpContext.Session.Remove("admin");
            HttpContext.Session.Remove("Cart");

            return RedirectToPage("/Index");
        }
    }
}
