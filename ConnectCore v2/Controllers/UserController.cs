#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConnectCore_v2.Data;
using ConnectCore_v2.Models;
using ConnectCore_v2.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ConnectCore_v2.Controllers
{
    public class UserController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public UserController(IDAL dal, UserManager<ApplicationUser> usermanager)
        {
            _dal = dal;
            _usermanager = usermanager;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: User/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // GET: User/Create
        public IActionResult Create()
        {
            //return View(new EventViewModel(_dal.GetLocations(), User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View(new CreateUserVM(_dal.GetLocations(),_dal.GetUserTypes() ,User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM vm, IFormCollection form)
        {
          
            try
            {
                _dal.CreateUser(form);
                //TempData["Alert"] = "Success! you created a new user";
                return RedirectToAction("Home","Index");
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return View(vm);
            }

        }

        public IActionResult CreateUserType()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUserType(IFormCollection form)
        {

            try
            {
                _dal.CreateUserType(form["Name"]);
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return View();
            }
            
        }


        public IActionResult CreateOtherUser(string id)
        {
            //return View(new EventViewModel(_dal.GetLocations(), User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View(new CreateUserVM(_dal.GetLocations(), _dal.GetUserTypes(),id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOtherUser(CreateUserVM vm, IFormCollection form)
        {

            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _dal.GetUserByAspNetId(userid);

                var loc = _dal.GetLoggedInUserLocation(user.Id);

                _dal.CreateOtherUser(form, loc);
                //TempData["Alert"] = "Success! you created a new user";
                return RedirectToAction("Home", "Index");
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return View(vm);
            }

        }

        // GET: User/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        // GET: User/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // POST: User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.User.FindAsync(id);
        //    _context.User.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.User.Any(e => e.Id == id);
        //}
    }
}
