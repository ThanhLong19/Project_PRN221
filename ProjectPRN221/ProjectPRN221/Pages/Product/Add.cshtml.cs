using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Product
{
    public class AddModel : PageModel
    {
        private readonly NorthwindContext db;
        [BindProperty]
        public ProjectPRN221.Models.Product product { get; set; }
        public AddModel(NorthwindContext db)
        {
            this.db = db;

        }
        public void OnGet()
        {
        }
    }
}
