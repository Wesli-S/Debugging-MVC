using CPW219_AspnetMVC_CRUD_Debugging.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPW219_AspnetMVC_CRUD_Debugging.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //return View(await _context.Product.ToListAsync());
            List<Product> product = await (from Product in _context.Product 
                                           select Product).ToListAsync();
            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();
                ViewData["Message"] = $"{product.Name} was added successfully!";
                return View();
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product productToDelete = await _context.Product.FindAsync(id);

            if(productToDelete == null)
            {
                return NotFound();
            }
            return View(productToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product productToDelete = await _context.Product.FindAsync(id);
            if (productToDelete != null)
            {
                _context.Remove(productToDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
