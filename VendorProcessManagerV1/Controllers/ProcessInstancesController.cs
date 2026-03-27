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
    public class ProcessInstancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessInstancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessInstances
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessInstances.ToListAsync());
        }

        // GET: ProcessInstances/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processInstance = await _context.ProcessInstances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processInstance == null)
            {
                return NotFound();
            }

            return View(processInstance);
        }

        // GET: ProcessInstances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessInstances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TemplateId,VendorCandidateId,InitiatedBy,StartDate,TargetEndDate,ActualEndDate,Status")] ProcessInstance processInstance)
        {
            if (ModelState.IsValid)
            {
                processInstance.Id = Guid.NewGuid();
                _context.Add(processInstance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processInstance);
        }

        // GET: ProcessInstances/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processInstance = await _context.ProcessInstances.FindAsync(id);
            if (processInstance == null)
            {
                return NotFound();
            }
            return View(processInstance);
        }

        // POST: ProcessInstances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TemplateId,VendorCandidateId,InitiatedBy,StartDate,TargetEndDate,ActualEndDate,Status")] ProcessInstance processInstance)
        {
            if (id != processInstance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processInstance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessInstanceExists(processInstance.Id))
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
            return View(processInstance);
        }

        // GET: ProcessInstances/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processInstance = await _context.ProcessInstances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processInstance == null)
            {
                return NotFound();
            }

            return View(processInstance);
        }

        // POST: ProcessInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processInstance = await _context.ProcessInstances.FindAsync(id);
            if (processInstance != null)
            {
                _context.ProcessInstances.Remove(processInstance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessInstanceExists(Guid id)
        {
            return _context.ProcessInstances.Any(e => e.Id == id);
        }
    }
}
