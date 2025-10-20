using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly WebApplicationContext _context;

        public object JsonRequestBehavior { get; private set; }

        public InvoicesController(WebApplicationContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Invoices.Include(i => i.Client).Include(i => i.Products).ToListAsync();
            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(p=>p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Invoice invoice, Client client, List<string> names, List<int> quantities)
        {
            var inv = _context.Invoices.FirstOrDefault(i=>i.Number==invoice.Number);
            if (inv != null)
            {
                TempData["Error"] = "An invoice with this number exists";
                return RedirectToAction("Index");
            }


            var cli = _context.Clients.Where(c => c.IdNumber == client.IdNumber).FirstOrDefault();
            if (cli != null)
            {
                invoice.Client = cli;
                _context.Entry(cli).State = EntityState.Unchanged;
            }


            for (int i = 0; i < names.Count; i++)
            {
                var p = new Product()
                {
                    Name = names.ElementAt(i),
                    Quantity = quantities.ElementAt(i),

                };
                var product = _context.Products.Where(c => c.Name == p.Name).FirstOrDefault();


                if (product != null)
                {
                    _context.Entry(product).State = EntityState.Unchanged;
                    invoice.Products.Add(product); ;
                    
                    
                }
                else
                {
                    TempData["Error"] = p.Name+" does not exist as a product";
                    return View("Create");
                }





                if (product.Quantity < p.Quantity)
                {
                    TempData["Error"] = "There are only " + product.Quantity + " " + product.Name + " in stock";
                    return RedirectToAction();
                }

                product.Quantity = product.Quantity - p.Quantity;
                _context.SaveChanges();



            }

            invoice.Value = CalculateValue(invoice.Products,quantities);
            _context.Add(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }




        private double CalculateValue(List<Product> values,List<int> quantities)

        {
            double sum = 0;
            for (int i = 0; i < values.Count; i++)
            {
                sum=sum+ values[i].Price*quantities[i];
            }
            return sum;
        }


        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", invoice.ClientId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Date,Value,ClientId")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", invoice.ClientId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.Include(i => i.Products).FirstOrDefaultAsync(i => i.Id == id);

            if (invoice != null)
            {
                foreach (var item in invoice.Products)
                {
                    item.Invoices.Remove(invoice);
                }
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        

    }
}
