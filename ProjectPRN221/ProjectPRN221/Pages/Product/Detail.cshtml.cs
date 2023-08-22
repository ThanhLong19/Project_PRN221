using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Product
{
    public class DetailModel : PageModel
    {
        private readonly NorthwindContext context;
        public Models.Product Product { get; set; }

        public DetailModel(NorthwindContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? pid)
        {
            bool productExists = await context.Products.AnyAsync(p => p.ProductId == pid);
            if (productExists)
            {
                Product = await context.Products.Include(p => p.Category).Where(p => p.ProductId == pid).SingleOrDefaultAsync();
                return Page();
            }
            else
            {
                return RedirectToPage("/Product/List");
            }
        }
    }
}
