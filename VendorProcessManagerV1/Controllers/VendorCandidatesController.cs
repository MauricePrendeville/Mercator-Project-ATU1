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
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class VendorCandidatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public VendorCandidatesController(ApplicationDbContext context, 
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VendorCandidates
        public async Task<IActionResult> Index(string sortOrder)
        {
            var vendors = _context.VendorCandidates
                .Include(v => v.ProcessInstances)
                .Include(v => v.Owner)
                .OrderByDescending(v => v.Name)
                .AsQueryable();

            vendors = sortOrder switch
            {
                "name_asc" => vendors.OrderBy(v => v.Name),
                "name_desc" => vendors.OrderByDescending(v => v.Name),
                "category_asc" => vendors.OrderBy(v => v.Category),
                "category_desc" => vendors.OrderByDescending(v => v.Category),
                "date_asc" => vendors.OrderBy(v => v.CreateDate),
                "date_desc" => vendors.OrderByDescending(v => v.CreateDate),
                "owner_asc" => vendors.OrderBy(v => v.Owner.LastName),
                "owner_desc" => vendors.OrderByDescending(v => v.Owner.LastName),
                "terms_asc" => vendors.OrderBy(v => v.PaymentTerms),
                "terms_desc" => vendors.OrderByDescending(v => v.PaymentTerms),
                _ => vendors.OrderBy(v => v.Name)
            };

            var vm = new VendorCandidateIndexViewModel
            {
                VendorCandidates = await vendors.ToListAsync(),
                CurrentSort = sortOrder,
                //vm.sortOrder ??= "name_asc";
                //ViewData["CurrentSort"] = SortOrder;
                VendorNameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc",
                CategorySort = sortOrder == "category_asc" ? "category_desc" : "category_asc",
                DateSort = sortOrder == "date_asc" ? "date_desc" : "date_asc",
                OwnerSort = sortOrder == "owner_asc" ? "owner_desc" : "owner_asc",
                TermsSort = sortOrder == "terms_asc" ? "terms_desc" : "terms_asc"
            };
            
            return View(vm);

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
        public async Task<IActionResult> Create([Bind("Id,Name,Category,CreateDate,LastModifiedDate,OwnerId,PaymentTerms")] VendorCandidate vendorCandidate)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                vendorCandidate.Id = Guid.NewGuid();
                vendorCandidate.OwnerId = currentUser?.Id;
                vendorCandidate.CreateDate = DateTime.Now;
                vendorCandidate.LastModifiedDate = DateTime.Now; 
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

            var vendorCandidate = await _context.VendorCandidates
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(v => v.Id == id);

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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Category,CreateDate,LastModifiedDate,OwnerId,PaymentTerms")] VendorCandidate vendorCandidate)
        {
            if (id != vendorCandidate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                vendorCandidate.LastModifiedDate = DateTime.Now; 
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
                .Include(v => v.Owner)
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
