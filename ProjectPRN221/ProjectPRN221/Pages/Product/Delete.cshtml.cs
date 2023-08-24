using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly NorthwindContext db = new NorthwindContext();

        [BindProperty]
        public string? mesString { get; set; }


        public DeleteModel(NorthwindContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(int? idDelete)
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
                if (idDelete == null)
                {
                    return RedirectToPage("./Manage");
                }
                else
                {
                    var prod = await db.Products.FindAsync(idDelete);
                    if (prod == null)
                        return BadRequest();
                    else
                    {
                        var listOrderdetail = await db.OrderDetails.Where(x => x.ProductId == idDelete).ToListAsync();
                        if (listOrderdetail.Count > 0)
                        {
                            foreach (var item in listOrderdetail)
                            {
                                db.OrderDetails.Remove(item);
                            }
                        }
                        mesString = "Product has been removed!";
                        db.Products.Remove(prod);
                        await db.SaveChangesAsync();
                        mesString = "Product have been removed!";
                        return Redirect("./Manage");
                    }
                }
            }
            return RedirectToPage("../Account/Login");
        }
    }
}
