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
    public class ProcessTemplateTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessTemplateTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessTemplateTasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTemplatesTasks.ToListAsync());
        }

        // GET: ProcessTemplateTasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTask = await _context.ProcessTemplatesTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTask == null)
            {
                return NotFound();
            }

            return View(processTemplateTask);
        }

        // GET: ProcessTemplateTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTemplateTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TemplateID,TaskId,Title,Description,ApproverId,ApproverTeam,ApprovalRequired,SortOrder,DefaultOwnerRole")] ProcessTemplateTask processTemplateTask)
        {
            if (ModelState.IsValid)
            {
                processTemplateTask.Id = Guid.NewGuid();
                _context.Add(processTemplateTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processTemplateTask);
        }

        // GET: ProcessTemplateTasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTask = await _context.ProcessTemplatesTasks.FindAsync(id);
            if (processTemplateTask == null)
            {
                return NotFound();
            }
            return View(processTemplateTask);
        }

        // POST: ProcessTemplateTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TemplateID,TaskId,Title,Description,ApproverId,ApproverTeam,ApprovalRequired,SortOrder,DefaultOwnerRole")] ProcessTemplateTask processTemplateTask)
        {
            if (id != processTemplateTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplateTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateTaskExists(processTemplateTask.Id))
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
            return View(processTemplateTask);
        }

        // GET: ProcessTemplateTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTask = await _context.ProcessTemplatesTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTask == null)
            {
                return NotFound();
            }

            return View(processTemplateTask);
        }

        // POST: ProcessTemplateTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTemplateTask = await _context.ProcessTemplatesTasks.FindAsync(id);
            if (processTemplateTask != null)
            {
                _context.ProcessTemplatesTasks.Remove(processTemplateTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateTaskExists(Guid id)
        {
            return _context.ProcessTemplatesTasks.Any(e => e.Id == id);
        }
    }
}
