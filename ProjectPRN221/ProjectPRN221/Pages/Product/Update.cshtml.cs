using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Product
{
    public class UpdateModel : PageModel
    {
        private readonly NorthwindContext db;
        [BindProperty]
        public ProjectPRN221.Models.Product product { get; set; }
        public UpdateModel(NorthwindContext db)
        {
            this.db = db;

        }
        public async Task<ActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                product = db.Products.Where(e => e.ProductId == id).SingleOrDefault();
                ViewData["SuppliersId"] = new SelectList(db.Suppliers.ToList(), "SupplierId", "CompanyName");
                ViewData["CategoriesId"] = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
                if (product == null)
                {
                    return NotFound();
                }

                return Page();
            }
        }
        public async Task<ActionResult> OnPostAsync(int? id)
        {
            //if (ModelState.IsValid)
            //{

            if (id == null)
                return NotFound();
            var prod = await db.Products.FindAsync(id);
            if (prod == null)
                return BadRequest();

            prod.ProductName = product.ProductName;
            prod.SupplierId = product.SupplierId;
            prod.CategoryId = product.CategoryId;
            prod.QuantityPerUnit = product.QuantityPerUnit;
            prod.UnitPrice = product.UnitPrice;
            prod.UnitsInStock = product.UnitsInStock;
            prod.Discontinued = product.Discontinued;
            prod.Description = product.Description;

            await db.SaveChangesAsync();
            return RedirectToPage("Manage");


            //}

            //return Page();
        }
    }
}
