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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ConnectCore_v2.Controllers
{
    [Authorize]
    public class EventController : Controller
    {    
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public EventController(IDAL dal, UserManager<ApplicationUser> usermanager)
        {
            _dal = dal;
            _usermanager = usermanager;
        }

        // GET: Event
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            return View(_dal.GetMyEvents(User.FindFirstValue(ClaimTypes.NameIdentifier))); 
        }

       

        // GET: Event/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _dal.GetUserByAspNetId(userid);

            List<User> userList = _dal.GetUsers(user.Location);
 
            return PartialView(new EventViewModel(_dal.GetLocations(),userList ,userid)); 
        }


        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel vm, IFormCollection form)
        {
            try
            {
                //get logged in user for location
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = _dal.GetUserByAspNetId(userid);

                //get user from dropdown list to assign to shift
                var assignToId = form["User"];
                ApplicationUser assignToUser = _dal.GetUserById(assignToId);

                //is form empty
                if (form["Event.Name"] == string.Empty || form["Event.StartTime"] == string.Empty || form["Event.EndTime"] == string.Empty || form["Event.Description"] == string.Empty)
                {
                    TempData["Alert"] = "Problem creating shift: Shift details are required ";
                    return RedirectToAction("Home", "ScheduleManager");
                }

                //is user null
                if (assignToUser == null)
                {
                    TempData["Alert"] = "Problem creating shift: Must assign a user ";
                    return RedirectToAction("Home", "ScheduleManager");
                }
                
               //is form valid - is end before start
               if(DateTime.Parse(form["Event.EndTime"].ToString()) <= DateTime.Parse(form["Event.StartTime"].ToString()))
                {
                    TempData["Alert"] = "Problem creating shift: End time must be later than start time ";
                    return RedirectToAction("Home", "ScheduleManager");
                }

                _dal.CreateEvent(form,user.Location, assignToUser);
               //TempData["Alert"] = "Success! you created a new event for: " + form["Event.Name"];
                return RedirectToAction("Home", "ScheduleManager");
            }catch(Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return View(vm);
            }
        }

        public IActionResult CreateRepeatingShift()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _dal.GetUserByAspNetId(userid);

            List<User> userList = _dal.GetUsers(user.Location);

            return PartialView(new EventViewModel(_dal.GetLocations(), userList, userid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRepeatingShift(EventViewModel vm, IFormCollection form)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _dal.GetUserByAspNetId(userid);

            var assignToId = form["User"];
            ApplicationUser assignToUser = _dal.GetUserById(assignToId);

            try
            {
                _dal.CreateRepeatingshift(form, user.Location, assignToUser);
                return RedirectToAction("Home", "ScheduleManager");
            }
            catch(Exception ex)
            {

            }


           return View(vm);
        }

        // GET: Event/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _dal.GetUserByAspNetId(userid);
            List<User> userList = _dal.GetUsers(user.Location);
            var cur = _dal.GetUserByAspNetId(@event.User.Id);

            var vm = new EventViewModel(@event, _dal.GetLocations(), userList, cur, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return PartialView(vm);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection form)
        {
           
            try
            {
                _dal.updateEvent(form);
                TempData["Alert"] = "Success! you modified a record for: " + form["Event.Name"];              
                return RedirectToAction("Home", "ScheduleManager");
            }
            catch(Exception ex)
            {

                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = _dal.GetUserByAspNetId(userid);
                List<User> userList = _dal.GetUsers(user.Location);
                var cur = _dal.GetUserByAspNetId(form["UserId"]);

                ViewData["Alert"] = "An error occurred: " + ex.Message;
                var vm = new EventViewModel(_dal.GetEvent(id), _dal.GetLocations(), userList, cur, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View(vm);
            }
               
        }

        // GET: Event/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
             _dal.DeleteEvent(id);
            TempData["Alert"] = "You delted an event";
            return RedirectToAction(nameof(Index));
        }
    
    }
}
