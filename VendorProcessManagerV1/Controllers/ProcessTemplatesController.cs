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
    public class ProcessTemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessTemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessTemplates
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTemplates.ToListAsync());
        }

        // GET: ProcessTemplates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // GET: ProcessTemplates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Category,Creator,CreateDate,IsActive,Version")] ProcessTemplate processTemplate)
        {
            if (ModelState.IsValid)
            {
                processTemplate.Id = Guid.NewGuid();
                _context.Add(processTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplates.FindAsync(id);
            if (processTemplate == null)
            {
                return NotFound();
            }
            return View(processTemplate);
        }

        // POST: ProcessTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Category,Creator,CreateDate,IsActive,Version")] ProcessTemplate processTemplate)
        {
            if (id != processTemplate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateExists(processTemplate.Id))
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
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // POST: ProcessTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTemplate = await _context.ProcessTemplates.FindAsync(id);
            if (processTemplate != null)
            {
                _context.ProcessTemplates.Remove(processTemplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateExists(Guid id)
        {
            return _context.ProcessTemplates.Any(e => e.Id == id);
        }
    }
}
