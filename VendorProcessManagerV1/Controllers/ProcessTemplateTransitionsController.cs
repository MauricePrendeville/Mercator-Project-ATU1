using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.ViewModels;

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
            return View(await _context.ProcessTemplateTransitions.ToListAsync());
        }

        // GET: ProcessTemplateTransitions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTransition = await _context.ProcessTemplateTransitions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processTemplateTransition == null)
            {
                return NotFound();
            }

            return View(processTemplateTransition);
        }

        // GET: ProcessTemplateTransitions/Create
        public async Task<IActionResult> Create(Guid fromTemplateTaskId)
        {
            var task = await _context.ProcessTemplatesTasks
                .Include(t =>t.ProcessTemplate)
                .Include(t => t.Transitions)
                .FirstOrDefaultAsync(t => t.Id == fromTemplateTaskId);

            if(task == null) 
                return NotFound();

            var existingDefault = task.Transitions
                .FirstOrDefault(t => t.IsDefault);

            var vm = new CreateProcessTemplateTransitionViewModel
            {
                FromProcessTemplateTaskId = fromTemplateTaskId,
                ProcessTemplateId = task.ProcessTemplateId,
                TaskTitle = task.Title,
                TemplateName = task.ProcessTemplate.Name,
                HasExistingDefault = existingDefault != null,
                ExistingDefaultLabel = existingDefault?.DisplayLabel,
                SortOrder = task.Transitions.Count + 1,
                ConditionTypeOptions = BuildConditionTypeOptions(), 
                TargetTaskOptions = await BuildTargetTaskOptions(
                    task.ProcessTemplateId, fromTemplateTaskId)
            };
            
            return View(vm);
        }

        // POST: ProcessTemplateTransitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProcessTemplateTransitionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.TargetTaskOptions = await BuildTargetTaskOptions(
                                            vm.ProcessTemplateId,
                                            vm.FromProcessTemplateTaskId);
                vm.ConditionTypeOptions = BuildConditionTypeOptions(vm.ConditionType);
                return View(vm); 

                //processTemplateTransition.Id = Guid.NewGuid();
                //_context.Add(processTemplateTransition);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            //return View(processTemplateTransition);

            if (vm.IsDefault)
            {
                var existingDefault = await _context.ProcessTemplateTransitions 
                    .FirstOrDefaultAsync(t =>
                        t.FromProcessTemplateTaskId == vm.FromProcessTemplateTaskId &&
                        t.IsDefault);

                if (existingDefault != null)
                {
                    existingDefault.IsDefault = false;
                    _context.Update(existingDefault);
                }
            }

            var transition = new ProcessTemplateTransition
            {
                Id = Guid.NewGuid(),
                FromProcessTemplateTaskId = vm.FromProcessTemplateTaskId,
                ToProcessTemplateTaskId = vm.ToProcessTemplateTaskId,
                DisplayLabel = vm.DisplayLabel, 
                SortOrder = vm.SortOrder,
                ConditionType = vm.ConditionType,
                ConditionExpression = vm.ConditionExpression,
                IsDefault = vm.IsDefault
            };

            _context.ProcessTemplateTransitions.Add(transition);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "ProcessTemplates",
                new { id = vm.ProcessTemplateId });
        }

        // GET: ProcessTemplateTransitions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplateTransition = await _context.ProcessTemplateTransitions.FindAsync(id);
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

            var processTemplateTransition = await _context.ProcessTemplateTransitions
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
            var processTemplateTransition = await _context.ProcessTemplateTransitions.FindAsync(id);
            if (processTemplateTransition != null)
            {
                _context.ProcessTemplateTransitions.Remove(processTemplateTransition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateTransitionExists(Guid id)
        {
            return _context.ProcessTemplateTransitions.Any(e => e.Id == id);
        }

        private SelectList BuildConditionTypeOptions(string? selected = null)
        {
            var types = new List<string>
            {
                "Approval",
                "Value Threshold",
                "Manual",
                "Automatic"
            };
            return new SelectList(
                types.Select(t => new { Value = t, Text = t }),
                "Value",
                "Text",
                selected
            );
        }

        private async Task<SelectList> BuildTargetTaskOptions(
            Guid templateId, 
            Guid currentTaskId, 
            Guid? selectedId = null)
        {
            var tasks = await _context.ProcessTemplatesTasks
                .Where(t => t.ProcessTemplateId == templateId &&
                t.Id != currentTaskId)
                .OrderBy(t => t.SortOrder)
                .Select(t => new { t.Id, t.Title })
                .ToListAsync();

            return new SelectList(tasks, "Id", "Title", selectedId); 
        }

        
    }
}
