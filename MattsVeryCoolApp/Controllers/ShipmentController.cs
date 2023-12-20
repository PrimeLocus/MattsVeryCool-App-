using MattsVeryCoolApp.Models;
using MattsVeryCoolApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MattsVeryCoolApp.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly LogisticsDbContext _context;

        public ShipmentController(LogisticsDbContext context)
        {
            _context = context;
        }

        // GET: Shipment
        public async Task<IActionResult> Index()
        {
            var shipments = await _context.Shipments.Include(s => s.Customer).ToListAsync();
            return View(shipments);
        }

        // GET: Shipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include the Customer navigation property
            var shipment = await _context.Shipments
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);

            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }


        // GET: Shipment/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Shipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Origin,Destination,Weight,Status,CustomerId")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                _context.Shipments.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Re-populate the dropdown list if returning to the view
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", shipment.CustomerId);
            return View(shipment);
        }


        // GET: Shipment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            return View(shipment);
        }

        // POST: Shipment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipmentId,Origin,Destination,Weight,Status")] Shipment shipment)
        {
            if (id != shipment.ShipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.ShipmentId))
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
            return View(shipment);
        }

        // GET: Shipment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // POST: Shipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.ShipmentId == id);
        }
    }
}
