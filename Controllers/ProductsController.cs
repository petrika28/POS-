using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Data;
using POS.Models;

namespace POS.Controllers
{
   

    public class ProductsController : Controller
    {
        private readonly WebApplicationContext _context;

        public ProductsController(WebApplicationContext context)
        {
            _context = context;
        }
        public IActionResult AddItem()
        {
            return View();


        }

        [HttpPost]
        public IActionResult AddItem(Product p)
        {
            if (ModelState.IsValid)
            {
                if (_context.Products.Where(prod => prod.Name.ToLower() == p.Name.ToLower()).ToList().IsNullOrEmpty())
                {

                    _context.Products.Add(p);
                    _context.SaveChanges();
                    return RedirectToAction("Items");


                }

                TempData["Error"] = "A product with the same name exists";
                return View();



            }
            ViewBag.ErrorMessage = "The data you have inputed is incorrect";
            return View("Items");
        }




        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            return View(product);

        }
        public IActionResult Deleted(Product p)
        {
            var product = _context.Products.Remove(p);
            _context.SaveChanges();
            return RedirectToAction("Items");

        }
        public async Task<IActionResult> Filter(string category)
        {
            var products = await _context.Products.Where(p => p.Category.ToLower() == category.ToLower()).ToListAsync();
            return View("Items",products);

        }
        public async Task<IActionResult> Items()
        {
            var low = await _context.Products.Where(p => p.Quantity < 5).ToListAsync();
            string list = " ";

            if (low.Any())
            {
                foreach (var item in low)
                {
                    list = list + item.Name + " ,";

                }
                list = list + " in low quantity";
                ViewBag.ErrorMessage = list;

            }
            var products = await _context.Products.ToListAsync();
            return View(products);

        }
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);

        }
        [HttpPost]
        public IActionResult Edit(Product p)
        {
            var product =  _context.Products.Find(p.Id);
            product.Name = p.Name;
            product.Quantity = p.Quantity;
            product.Price = p.Price;
            product.Category = p.Category;
            _context.SaveChanges();
            return RedirectToAction("Items");

        }
        public async Task<IActionResult> LowQuantity()
        {
            var products = await _context.Products.Where(p => p.Quantity < 5).ToListAsync();
            return View(products);

        }

        [HttpGet]
        public IActionResult SearchProducts(string term)
        {
            // Check if the term is null or empty
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<Product>());
            }

            //
         var matchingProductNames = _context.Products
        .Where(p => p.Name.ToLower().Contains(term.ToLower()))
        .Select(p => p.Name)  // Select only the Name of the product
        .ToList();

            return Json(matchingProductNames);
            
        }






    }


}

