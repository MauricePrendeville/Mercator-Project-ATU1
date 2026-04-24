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
    public class ProcessTasks1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessTasks1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessTasks1
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessTasks.ToListAsync());
        }

        // GET: ProcessTasks1/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTask = await _context.ProcessTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTask == null)
            {
                return NotFound();
            }

            return View(processTask);
        }

        // GET: ProcessTasks1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTasks1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Task_Id,Title,Description,CreatedDate,UpdatedDate,CompletedDate,Creator,Owner,ApproverTeam,Approver,ApproveDate,ApproveStatus,TaskStatus,IsCompleted,RequiresApproval")] ProcessTask processTask)
        {
            if (ModelState.IsValid)
            {
                processTask.Id = Guid.NewGuid();
                _context.Add(processTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processTask);
        }

        // GET: ProcessTasks1/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTask = await _context.ProcessTasks.FindAsync(id);
            if (processTask == null)
            {
                return NotFound();
            }
            return View(processTask);
        }

        // POST: ProcessTasks1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Task_Id,Title,Description,CreatedDate,UpdatedDate,CompletedDate,Creator,Owner,ApproverTeam,Approver,ApproveDate,ApproveStatus,TaskStatus,IsCompleted,RequiresApproval")] ProcessTask processTask)
        {
            if (id != processTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTaskExists(processTask.Id))
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
            return View(processTask);
        }

        // GET: ProcessTasks1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTask = await _context.ProcessTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTask == null)
            {
                return NotFound();
            }

            return View(processTask);
        }

        // POST: ProcessTasks1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var processTask = await _context.ProcessTasks.FindAsync(id);
            if (processTask != null)
            {
                _context.ProcessTasks.Remove(processTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTaskExists(Guid id)
        {
            return _context.ProcessTasks.Any(e => e.Id == id);
        }
    }
}
