using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPRN221.Models;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

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
        public async Task<ActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
                //load du lieu cua cities
                ViewData["SuppliersId"] = new SelectList(db.Suppliers.ToList(), "SupplierId", "CompanyName");
                ViewData["CategoriesId"] = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
                return Page();
            }
            return RedirectToPage("../Account/Login");

        }

        public async Task<ActionResult> OnPostAsync()
        {
            //if (ModelState.IsValid)
            //{
            if (HttpContext.Session.GetString("admin") != null)
            {
                Console.WriteLine(product.Description);
                //Save data into DB
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToPage("Manage");


            }
            return RedirectToPage("../Account/Login");
        }
    }
}