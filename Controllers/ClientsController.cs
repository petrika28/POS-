using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;

namespace WebApplication5.Controllers
{
    public class ClientsController : Controller
    {
        private readonly WebApplicationContext _context;
        public ClientsController(WebApplicationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.ToListAsync();
            return View(clients);
        }
        public IActionResult Delete(int id)
        {
            var client=_context.Clients.FirstOrDefault(x => x.Id == id);
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
