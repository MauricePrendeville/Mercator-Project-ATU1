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
    public class ProcessTaskTransitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessTaskTransitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessTaskTransitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTaskTransition.ToListAsync());
        }

        // GET: ProcessTaskTransitions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTaskTransition = await _context.ProcessTaskTransition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTaskTransition == null)
            {
                return NotFound();
            }

            return View(processTaskTransition);
        }

        // GET: ProcessTaskTransitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTaskTransitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProcessInstanceId,ProcessTemplateTrasitionId,FromProcessStepId,ToProcessStepId,ActionUserId,ActionDate,ActionValue")] ProcessTaskTransition processTaskTransition)
        {
            if (ModelState.IsValid)
            {
                processTaskTransition.Id = Guid.NewGuid();
                _context.Add(processTaskTransition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processTaskTransition);
        }

        // GET: ProcessTaskTransitions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTaskTransition = await _context.ProcessTaskTransition.FindAsync(id);
            if (processTaskTransition == null)
            {
                return NotFound();
            }
            return View(processTaskTransition);
        }

        // POST: ProcessTaskTransitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProcessInstanceId,ProcessTemplateTrasitionId,FromProcessStepId,ToProcessStepId,ActionUserId,ActionDate,ActionValue")] ProcessTaskTransition processTaskTransition)
        {
            if (id != processTaskTransition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTaskTransition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTaskTransitionExists(processTaskTransition.Id))
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
            return View(processTaskTransition);
        }

        // GET: ProcessTaskTransitions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTaskTransition = await _context.ProcessTaskTransition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTaskTransition == null)
            {
                return NotFound();
            }

            return View(processTaskTransition);
        }

        // POST: ProcessTaskTransitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTaskTransition = await _context.ProcessTaskTransition.FindAsync(id);
            if (processTaskTransition != null)
            {
                _context.ProcessTaskTransition.Remove(processTaskTransition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTaskTransitionExists(Guid id)
        {
            return _context.ProcessTaskTransition.Any(e => e.Id == id);
        }
    }
}
