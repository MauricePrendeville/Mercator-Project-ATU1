using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.DTO;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.Services;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    /// <summary>
    /// Provides actions for viewing, creating, editing, deleting process instances within the 
    /// application. 
    /// </summary>
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

        /// <summary>
        /// GET method for showing list of Process Instances. Sorting and filtering 
        /// functionality are included
        /// </summary>
        /// <param name="vm"> The viewmodel that is passed to the method.</param>
        /// <returns>A view for display taking filters and sorting into account.</returns>
        public async Task<IActionResult> Index (ProcessInstanceIndexViewModel vm)
        {
            vm.SortOrder ??= "name_asc";

            vm.InstanceNameSort = vm.SortOrder == "name_asc" ? "name_desc" : "name_asc";
            vm.TemplateSort = vm.SortOrder == "template_asc" ? "template_desc" : "template_asc";
            vm.VendorSort = vm.SortOrder == "vendor_asc" ? "vendor_desc" : "vendor_asc";
            vm.DateSort = vm.SortOrder == "date_asc" ? "date_desc" : "date_asc";
            vm.InitiatorSort = vm.SortOrder == "initiator_asc" ? "initiator_desc" : "initiator_asc";
            vm.StatusSort = vm.SortOrder == "status_asc" ? "status_desc" : "status_asc";

            var teams = await _context.ProcessTasks
                .Where(t => !string.IsNullOrEmpty(t.ApproverTeam))
                .Select(t => t.ApproverTeam)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            vm.TeamList = teams.Select(t => new SelectListItem
            {
                Value = t, 
                Text = t,
                Selected = t == vm.TeamFilter
            });

            var instances = _context.ProcessInstances
                .Include(i => i.ProcessTemplate)
                .Include(i => i.InitiatedBy)
                .Include(i => i.VendorCandidate)
                .Include(i => i.Tasks)
                .AsQueryable();


            if (!string.IsNullOrEmpty(vm.TeamFilter))
            {
                instances = instances
                    .Where(i => i.Tasks.Any(t => t.ApproverTeam ==
                    vm.TeamFilter));
            }

            instances = vm.SortOrder switch
            {
                "name_asc" => instances.OrderBy(p => p.InstanceName),
                "name_desc" => instances.OrderByDescending(p => p.InstanceName),
                "template_asc" => instances.OrderBy(p => p.ProcessTemplate.Name),
                "template_desc" => instances.OrderByDescending(p => p.ProcessTemplate.Name),
                "vendor_asc" => instances.OrderBy(p => p.VendorCandidate.Name),
                "vendor_desc" => instances.OrderByDescending(p => p.VendorCandidate.Name),
                "date_asc" => instances.OrderBy(p => p.CreatedDate),
                "date_desc" => instances.OrderByDescending(p => p.CreatedDate),
                "initiator_asc" => instances.OrderBy(p => p.InitiatedBy.LastName),
                "initiator_desc" => instances.OrderByDescending(p => p.InitiatedBy.LastName),
                "status_asc" => instances.OrderBy(p => p.Status),
                "status_desc" => instances.OrderByDescending(p => p.Status),
                _ => instances.OrderBy(p => p.InstanceName)
            };

            vm.Instances = instances.ToList();


            return View(vm);

            /*
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = teamFilter;

            ViewData["InstanceNameSort"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["TemplateSort"] = sortOrder == "template_asc" ? "template_desc" : "template_asc";
            ViewData["VendorSort"] = sortOrder == "vendor_asc" ? "vendor_desc" : "vendor_asc";
            ViewData["DateSort"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["InitiatorSort"] = sortOrder == "initiator_asc" ? "initiator_desc" : "initiator_asc";
            ViewData["StatusSort"] = sortOrder == "status_asc" ? "status_desc" : "status_asc";

            var teams = await _context.ProcessTasks
                .Where(t => !string.IsNullOrEmpty(t.ApproverTeam))
                .Select(t => t.ApproverTeam)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            ViewData["TeamList"] = new SelectList(teams); 

            

            if (!string.IsNullOrEmpty(teamFilter))
            {
                instances = instances.Where(i =>
                i.Tasks.Any(t => t.ApproverTeam == teamFilter));
            }

            instances = sortOrder switch
            {
                "name_asc" => instances.OrderBy(p => p.InstanceName),
                "name_desc" => instances.OrderByDescending(p => p.InstanceName),
                "template_asc" => instances.OrderBy(p => p.ProcessTemplate.Name),
                "template_desc" => instances.OrderByDescending(p => p.ProcessTemplate.Name),
                "vendor_asc" => instances.OrderBy(p => p.VendorCandidate.Name),
                "vendor_desc" => instances.OrderByDescending(p => p.VendorCandidate.Name),
                "date_asc" => instances.OrderBy(p => p.CreatedDate),
                "date_desc" => instances.OrderByDescending(p => p.CreatedDate),
                "initiator_asc" => instances.OrderBy(p => p.InitiatedBy.LastName),
                "initiator_desc" => instances.OrderByDescending(p => p.InitiatedBy.LastName),
                "status_asc" => instances.OrderBy(p => p.Status),
                "status_desc" => instances.OrderByDescending(p => p.Status),
                _ => instances.OrderBy(p => p.InstanceName)
            };
            
            return View(await instances.ToListAsync());
            */
        }

        // GET: ProcessInstances/Details/5
        /// <summary>
        /// GET method for displaying details of a process instance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View for display that includes associated tasks, vendor candidates, 
        /// and the gantt chart details. </returns>
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

            var vm = new DetailsProcessInstanceViewModel
            {
                ProcessInstance = processInstance,
                GanttTasks = BuildInstanceGantt(processInstance)
            };
                        
            return View(vm);
        }

        // GET: ProcessInstances/Create
        /// <summary>
        /// GET method for creating a ProcessInstance 
        /// </summary>
        /// <param name="id">The id number of the Process Template used to create the 
        /// Instance</param>
        /// <returns>The viewmodel for the new instance is successful. The details of the Template 
        /// if there are no tasks associated with it or not found if the template does not 
        /// exist </returns>
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
        /// <summary>
        /// POST action to create a ProcessInstance
        /// </summary>
        /// <param name="processInstance"> The details to create a new process instance.</param>
        /// <returns>A redirect to the Index if successful, otherwise the view with the 
        /// current viewmodel. </returns>
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
        /// <summary>
        /// The GET action for Editing ProcessInstances
        /// </summary>
        /// <param name="id">The Id number of the ProcessInstance</param>
        /// <returns>The viewmodel with the updated details if successful, otherwise 
        /// a NotFound result if the Id number or the process instance are null</returns>
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
        /// <summary>
        /// GET method to Start a Process Instance. This is the primary way to create 
        /// a process instance based on the details of a process template.
        /// </summary>
        /// <param name="id">The Id number of the Procee Template</param>
        /// <returns>The viewmodel for the new instance is successful. 
        /// The details of the Template if there are no tasks associated 
        /// with it, or NotFound result if the template does not exist</returns>
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
        /// <summary>
        /// POST method for starting a Process Instance.
        /// </summary>
        /// <param name="vm">The viewmodel for the new Process Instance</param>
        /// <returns>A redirect to the details of the instance if successful, otherwise 
        /// it returns the viewmodel or an unauthorized result. </returns>
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
        /// <summary>
        /// POST action for editing process instances.
        /// </summary>
        /// <param name="id">The Id of the Process Instance.</param>
        /// <param name="vm">The viewmodel of the Process Instance</param>
        /// <returns>A redirect to the Index view if successful, otherwise the viewmodel,
        /// or a NotFound result.</returns>
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
        /// <summary>
        /// GET method for deleting process instances. 
        /// </summary>
        /// <param name="id">The Id of the Process Instance</param>
        /// <returns>The processinstance view if successful, otherwise the 
        /// NotFound result.</returns>
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
        /// <summary>
        /// POST method for deleting process instances.
        /// </summary>
        /// <param name="id">The Id of the process instance</param>
        /// <returns>A redirect to the Index view.</returns>
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
        
        /// <summary>
        /// A method to check the existence of a process instance.
        /// </summary>
        /// <param name="id">The Id of the process instance.</param>
        /// <returns>The process instance.</returns>
        private bool ProcessInstanceExists(Guid id)
        {
            return _context.ProcessInstances.Any(e => e.Id == id);
        }

        /// <summary>
        /// Makes the list of status options. 
        /// </summary>
        /// <param name="selected">The selected status</param>
        /// <returns>The list of available statuses</returns>
        private SelectList BuildStatusOptions(
            ProcessInstanceStatus? selected = null)
        {
            var statuses = Enum.GetValues(typeof(ProcessInstanceStatus))
                .Cast<ProcessInstanceStatus>()
                .Select(s => new { Value = (int)s, Text = s.ToString() })
                .ToList();

            return new SelectList(statuses, "Value", "Text", (int?)selected); 
        }

        /// <summary>
        /// Creates data for a Frappe Gantt chart using a data transfer objects (DTOs). 
        /// </summary>
        /// <param name="instance">The process instance object</param>
        /// <returns>A list of Frappe Gantt chart DTOs</returns>
        private List<GanttDTO> BuildInstanceGantt(ProcessInstance instance)
        {
            var result = new List<GanttDTO>();

            var orderedTasks = instance.Tasks
                .OrderBy(t => t.SortOrder)
                .ToList();

            foreach (var task in orderedTasks)
            {
                var start = task.StartedDate ?? instance.StartDate ?? 
                    DateTime.Now;
                var end = task.CompletedDate ?? start.AddDays(1);

                result.Add(new GanttDTO
                {
                    id = task.Id.ToString(),
                    name = $"{task.SortOrder}.   {task.Title}",
                    start = start.ToString("yyyy-MM-dd"),
                    end = end.ToString("yyyy-MM-dd"),
                    progress = GetProgress(task),
                    customClass = GetCssClass(task),
                    dependencies = null
                    
                });
            }
            return result; 
        }

        /// <summary>
        /// Calculates a progress percentage for gantt chart DTOs.
        /// </summary>
        /// <param name="task">The process task</param>
        /// <returns>A percentage based on the status of the task.</returns>
        private int GetProgress(ProcessTask task)
        {
            if (task.CompletedDate.HasValue)
                return 100;
            if (task.StartedDate.HasValue)
                return 50;
            
            return 0;
        }
       
        /// <summary>
        /// Sets the CSS value for the task based on the completion status.
        /// </summary>
        /// <param name="task">The process task.</param>
        /// <returns>The CSS value depending on the completion status.</returns>
        private string GetCssClass(ProcessTask task) 
        {
            if (task.CompletedDate.HasValue) return "task-complete"; 
            if (task.StartedDate.HasValue) return "task-active";
                return "task-pending"; 
        }
    }
}
