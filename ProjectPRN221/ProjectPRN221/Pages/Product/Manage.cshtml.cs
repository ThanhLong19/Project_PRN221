using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Product
{
    public class ManageModel : PageModel
    {
        private readonly NorthwindContext db = new NorthwindContext();

        [BindProperty]
        public List<ProjectPRN221.Models.Product> products { set; get; }
        [BindProperty]
        public List<Category> categories { set; get; }
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [BindProperty]
        public string? mesString { get; set; }


        public ManageModel(NorthwindContext db)
        {
            this.db = db;
        }


        public async Task<IActionResult> OnGetAsync(int? cid, string? mes)
        {
            if (!String.IsNullOrEmpty(mes))
                mesString = mes;
            else
            {
                mesString = null;
            }

            categories = await db.Categories.ToListAsync();

            var prod = from m in db.Products select m;

            if (HttpContext.Session.GetString("admin") != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    prod = prod.Where(s => s.ProductName.Contains(SearchString));
                }

                if (cid != null)
                {
                    prod = prod.Where(s => s.Category.CategoryId == cid);
                }

                if (!string.IsNullOrEmpty(SearchString) && cid != null)
                {
                    prod = prod.Where(s => s.Category.CategoryId == cid && s.Category.CategoryId == cid);
                }

                products = await prod.ToListAsync();

                return Page();
            }
            return RedirectToPage("../Account/Login");
        }
    }
}
