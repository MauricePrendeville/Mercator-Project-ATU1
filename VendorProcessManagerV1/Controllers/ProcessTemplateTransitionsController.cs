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
    public class ProcessTemplateTransitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessTemplateTransitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessTemplateTransitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTemplateTransition.ToListAsync());
        }

        // GET: ProcessTemplateTransitions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTransition = await _context.ProcessTemplateTransition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTransition == null)
            {
                return NotFound();
            }

            return View(processTemplateTransition);
        }

        // GET: ProcessTemplateTransitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTemplateTransitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProcessTemplateId,FromProcessTemplateTaskId,ToProcessTemplateTaskId,DisplayLabel,SortOrder,ConditionType,ConditionExpression,IsDefault")] ProcessTemplateTransition processTemplateTransition)
        {
            if (ModelState.IsValid)
            {
                processTemplateTransition.Id = Guid.NewGuid();
                _context.Add(processTemplateTransition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processTemplateTransition);
        }

        // GET: ProcessTemplateTransitions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTransition = await _context.ProcessTemplateTransition.FindAsync(id);
            if (processTemplateTransition == null)
            {
                return NotFound();
            }
            return View(processTemplateTransition);
        }

        // POST: ProcessTemplateTransitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProcessTemplateId,FromProcessTemplateTaskId,ToProcessTemplateTaskId,DisplayLabel,SortOrder,ConditionType,ConditionExpression,IsDefault")] ProcessTemplateTransition processTemplateTransition)
        {
            if (id != processTemplateTransition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplateTransition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateTransitionExists(processTemplateTransition.Id))
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
            return View(processTemplateTransition);
        }

        // GET: ProcessTemplateTransitions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTransition = await _context.ProcessTemplateTransition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTransition == null)
            {
                return NotFound();
            }

            return View(processTemplateTransition);
        }

        // POST: ProcessTemplateTransitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTemplateTransition = await _context.ProcessTemplateTransition.FindAsync(id);
            if (processTemplateTransition != null)
            {
                _context.ProcessTemplateTransition.Remove(processTemplateTransition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateTransitionExists(Guid id)
        {
            return _context.ProcessTemplateTransition.Any(e => e.Id == id);
        }
    }
}
