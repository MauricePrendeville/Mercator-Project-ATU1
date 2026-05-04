using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.Services;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class ProcessInstancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProcessInstanceService _processInstanceService;

        public ProcessInstancesController(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            IProcessInstanceService processInstanceService)
        {
            _context = context;
            _userManager = userManager;
            _processInstanceService = processInstanceService;
        }

        // GET: ProcessInstances
        public async Task<IActionResult> Index()
        {
            var instances = await _context.ProcessInstances
                .Include(i => i.ProcessTemplate)
                .Include(i => i.InitiatedBy)
                .Include(i => i.VendorCandidate)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();
            
            return View(instances);
        }

        // GET: ProcessInstances/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processInstance = await _context.ProcessInstances
                .Include(i => i.ProcessTemplate)
                .Include(i => i.VendorCandidate)
                .Include(i => i.InitiatedBy)
                .Include(i => i.Tasks.OrderBy(t => t.SortOrder))
                    .ThenInclude(t =>t.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserTeam = currentUser?.Team; 

            if (processInstance == null)
            {
                return NotFound();
            }

            return View(processInstance);
        }

        // GET: ProcessInstances/Create
        public async Task<IActionResult> Create(Guid id)
        {
            var template = await _context.ProcessTemplates
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return NotFound();

            if (!template.Tasks.Any())
            {
                TempData["Error"] = "Cannot start an instance. Template has no tasks";
                return RedirectToAction("Details", new { id });
            }

            var vm = new StartProcessInstanceViewModel
            {
                ProcessTemplateId = template.Id,
                TemplateName = template.Name,
                TaskCount = template.Tasks.Count,
                InstanceName = template.Name + "-" + DateTime.Now.ToString("dd MMM yyyy")
            };

            return View(vm);
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

            var processInstance = await _context.ProcessInstances
                .Include(i => i.ProcessTemplate)
                .Include(i => i.VendorCandidate)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (processInstance == null)
            {
                return NotFound();
            }

            var vm = new EditProcessInstanceViewModel
            {
                Id = processInstance.Id,
                Name = processInstance.InstanceName,
                TemplateName = processInstance.ProcessTemplate?.Name,
                VendorName = processInstance.VendorCandidate?.Name,
                CreatedDate = processInstance.CreatedDate,
                StartDate = processInstance.StartDate,
                TargetEndDate = processInstance.TargetEndDate,
                ActualEndDate = processInstance.ActualEndDate,
                Status = processInstance.Status,
                StatusOptions = BuildStatusOptions(processInstance.Status)
            }; 

            return View(vm);
        }

        //GET: ProcessTemplates/StartInstance/templateId
        public async Task<IActionResult> StartInstance(Guid id)
        {
            var template = await _context.ProcessTemplates
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
                return NotFound();

            if (!template.Tasks.Any())
            {
                TempData["Error"] = "Cannot start an instance this template has no tasks.";
                return RedirectToAction("Details", new { id });
            }

            var vm = new StartProcessInstanceViewModel
            {
                ProcessTemplateId = template.Id,
                TemplateName = template.Name,
                TaskCount = template.Tasks.Count,
                SuggestedName = template.Name + "-" +
                                DateTime.Now.ToString("dd MM yyyy"), 
                VendorCandidateOptions = new SelectList(
                    await _context.VendorCandidates
                        .OrderBy(v => v.Name)
                        .ToListAsync(),
                    "Id", 
                    "Name"
                 )
            };

            return View(vm);
        }

        //POST ProcessTemplates/StartInstance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartInstance(StartProcessInstanceViewModel vm)
        {
            if (!ModelState.IsValid)
            {   
                vm.VendorCandidateOptions = new SelectList(
                    await _context.VendorCandidates
                    .OrderBy(v => v.Name)
                    .ToListAsync(), 
                    "Id", 
                    "Name"
                    );
                return View(vm);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            try
            {
                var instance = await _processInstanceService.StartInstanceAsync(
                    vm.ProcessTemplateId,
                    vm.InstanceName, 
                    vm.VendorCandidateId,
                    currentUser.Id);

                TempData["Success"] = "Process instance started successfully.";

                //redirect
                return RedirectToAction("Details", 
                    new { id = instance.Id });
            }

            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }
        // POST: ProcessInstances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditProcessInstanceViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.StatusOptions = BuildStatusOptions(vm.Status);
                return View(vm); 
            }

            var processInstance = await _context.ProcessInstances.FindAsync(id);
            if (processInstance == null)
                return NotFound();

            processInstance.StartDate       = vm.StartDate;
            processInstance.TargetEndDate   = vm.TargetEndDate;
            processInstance.ActualEndDate   = vm.ActualEndDate;
            processInstance.Status          = vm.Status;

            if (vm.Status == ProcessInstanceStatus.Completed &&
                processInstance.ActualEndDate == null)
                processInstance.ActualEndDate = DateTime.Now; 

            try
            {
                _context.Update(processInstance);
                await _context.SaveChangesAsync();
            }
            
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ProcessInstances.Any(i => i.Id == vm.Id))
                    return NotFound();
                throw;                
            }
            
            return RedirectToAction(nameof(Index));
                        
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

        private SelectList BuildStatusOptions(
            ProcessInstanceStatus? selected = null)
        {
            var statuses = Enum.GetValues(typeof(ProcessInstanceStatus))
                .Cast<ProcessInstanceStatus>()
                .Select(s => new { Value = (int)s, Text = s.ToString() })
                .ToList();

            return new SelectList(statuses, "Value", "Text", (int?)selected); 
        }
    }
}
