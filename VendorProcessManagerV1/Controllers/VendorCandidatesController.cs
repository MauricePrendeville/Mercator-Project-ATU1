using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.Controllers
{
    public class VendorCandidatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorCandidatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VendorCandidates
        public async Task<IActionResult> Index()
        {
            var vendors = await _context.VendorCandidates
                .Include(v => v.ProcessInstances)
                .OrderBy(v => v.Name)
                .ToListAsync(); 

            return View(vendors);
        }

        // GET: VendorCandidates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorCandidate = await _context.VendorCandidates
                .Include(v => v.ProcessInstances)
                    .ThenInclude(i => i.ProcessTemplate)
                .Include(v => v.ProcessInstances)
                    .ThenInclude(i => i.InitiatedBy)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendorCandidate == null)
            {
                return NotFound();
            }

            return View(vendorCandidate);
        }

        // GET: VendorCandidates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VendorCandidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,CreateDate,LastModifiedDate,Creator,PaymentTerms")] VendorCandidate vendorCandidate)
        {
            if (ModelState.IsValid)
            {
                vendorCandidate.Id = Guid.NewGuid();
                _context.Add(vendorCandidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendorCandidate);
        }

        // GET: VendorCandidates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorCandidate = await _context.VendorCandidates.FindAsync(id);
            if (vendorCandidate == null)
            {
                return NotFound();
            }
            return View(vendorCandidate);
        }

        // POST: VendorCandidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Category,CreateDate,LastModifiedDate,Creator,PaymentTerms")] VendorCandidate vendorCandidate)
        {
            if (id != vendorCandidate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendorCandidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorCandidateExists(vendorCandidate.Id))
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
            return View(vendorCandidate);
        }

        // GET: VendorCandidates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorCandidate = await _context.VendorCandidates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendorCandidate == null)
            {
                return NotFound();
            }

            return View(vendorCandidate);
        }

        // POST: VendorCandidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vendorCandidate = await _context.VendorCandidates.FindAsync(id);
            if (vendorCandidate != null)
            {
                _context.VendorCandidates.Remove(vendorCandidate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendorCandidateExists(Guid id)
        {
            return _context.VendorCandidates.Any(e => e.Id == id);
        }
    }
}
