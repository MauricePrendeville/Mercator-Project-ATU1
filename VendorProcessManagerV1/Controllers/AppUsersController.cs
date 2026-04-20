using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.ViewModels;

namespace VendorProcessManagerV1.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        
        
        public AppUsersController(UserManager<AppUser> userManager, 
                                    ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;            
        }

        // GET: AppUsers
        public async Task<IActionResult> Index()
        {
            //var users = _userManager.Users.ToListAsync();
            var users = await Task.Run(() => _userManager.Users.ToList());
            //_context.AppUsers
              //  .OrderBy(u => u.LastName)
                //.ToListAsync();
            return View(users);
                //await _context.AppUsers.ToListAsync());
        }

        // GET: ProcessTasks/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id);
                //await _context.AppUsers
                //.FirstOrDefaultAsync(m => m.Id == id);
            
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: ProcessTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName," +
            "LastName,CreatedDate,UpdatedDate," +
            "Team, UserType")] AppUser appUser, string password)
        {
            //if (ModelState.IsValid)
            //{
            //    appUser.Id = Guid.NewGuid();
            //    _context.Add(appUser);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            if (!ModelState.IsValid) {
                //await PopulateDropdowns(appUser);
                return View(appUser); 
            }
            appUser.CreatedDate = DateTime.Now;
            appUser.UpdatedDate = DateTime.Now;

            var result = await _userManager.CreateAsync(appUser, password);

            if (result.Succeeded) 
            { 
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors) 
            { 
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(appUser);

        }

        // GET: ProcessTasks/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            var vm = new EditAppUserViewModel
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                Team = appUser.Team,
                UserType = appUser.UserType
            };

            return View(vm);
        }

        // POST: ProcessTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditAppUserViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(vm); 
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Team = vm.Team;
            user.UserType = vm.UserType;
            user.UpdatedDate = DateTime.Now; 

            if(user.Email != vm.Email) 
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, vm.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(vm);
                }            
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(vm);           
        }

        // GET: ProcessTasks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: ProcessTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            
            if (appUser == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(appUser);

            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(appUser);
        }

       
    }
}
