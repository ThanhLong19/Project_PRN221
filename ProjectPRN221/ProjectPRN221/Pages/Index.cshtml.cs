using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NorthwindContext context;
        [BindProperty]
        public List<Category> Categories { get; set; }
        [BindProperty]
        public List<Models.Product> Hot { get; set; }

        public IndexModel(NorthwindContext context)
        {
            this.context = context; 
        }
        

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await context.Categories.ToListAsync();
            Hot = await context.Products.Where(p => p.UnitsOnOrder > 0 && p.UnitsInStock > 0).OrderByDescending(p => p.UnitsOnOrder).Take(12).ToListAsync();
            return Page();
        }
    }
}