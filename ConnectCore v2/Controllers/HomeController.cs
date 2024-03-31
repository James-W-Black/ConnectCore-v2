using ConnectCore_v2.Data;
using ConnectCore_v2.Helpers;
using ConnectCore_v2.Models;
using ConnectCore_v2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Omu.AwesomeMvc;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;


namespace ConnectCore_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDAL _idal;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly SignInManager<ApplicationUser> _signInManager;
   
        public HomeController(ILogger<HomeController> logger, IDAL idal, UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _idal = idal;
            _usermanager = usermanager;
            _signInManager = signInManager;
            
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));

                return View(user);
            }
            //return View();
            //return RedirectToRoute("/Login");

            return LocalRedirect("/Identity/Account/Login");
        }

        [Authorize]
        public IActionResult ScheduleManager()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }

            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetLocations());
            var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetEvents(user.Location));
            return View();
        }

        [HttpPost]
        public IActionResult CancelHolidays(int Id)
        {
            Holiday hol = _idal.GetholById(Id);
            
            _idal.CancelHoliday(hol);

            return RedirectToAction("UserDetails");
        }

        public IActionResult PopupCancelHol(int id)
        {
            Holiday hol = _idal.GetholById(id);

            return PartialView(hol);
        }

        public async Task<IActionResult> CreateHoliday(UserDetailsVM vm,IFormCollection form)
        {                                
            //check if null form
            if(form["Holiday.StartTime"] == string.Empty || form["Holiday.EndTime"] == string.Empty || form["Holiday.Description"] == string.Empty)
            {
                TempData["Alert"] = "All fields are required";
                return RedirectToAction(null);
            }

            DateTime start = Convert.ToDateTime(form["Holiday.StartTime"]);
            DateTime end = Convert.ToDateTime(form["Holiday.EndTime"]);

            //check valid date - end date not earlier than start
            if (end < start)
            {
                TempData["Alert"] = "End date cannot be before start date" ;
                return RedirectToAction(null);                              
            }

            //check if already existing holiday exists for that date or if there is intersecting 
            User user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Holiday> hols = _idal.GetHolsForUser(user);

            foreach(Holiday hol in hols)
            {
                //if either start/ end time is both > hol.start and < hol.end it means it is overlapping
                if( (start.Date >= hol.StartTime.Date && start.Date <= hol.EndTime.Date) || (end.Date >= hol.StartTime.Date && end.Date <= hol.EndTime.Date))
                {
                    TempData["Alert"] = "This request overlaps with another holiday you have requested";
                    return RedirectToAction(null);
                }
            }

            try
            {
                _idal.CreateHoliday(form);
                TempData["Alert"] = "Success! you created a holiday";
                return RedirectToAction("Home","UserDetails");
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Home", "UserDetails");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateUserDetails(UserDetailsVM vm, IFormCollection form)
        {
            //dal update user
            //temp data success
            //update roles of said user
            //refresh details page

            //if error
            //view data error
            //return vm

            //var user = _idal.GetUserByAspNetId(vm.UserId);
            var r = form["Role"];
            var userid = form["UserId"];
            List<IdentityRole> roles = _idal.GetRoles().ToList();

            var user = await _usermanager.FindByIdAsync(userid);

            foreach(var role in roles)
            {
                try
                {
                    await _usermanager.RemoveFromRoleAsync(user, role.Name);
                }
                catch (Exception ex)
                {
                    
                }
            }

            foreach (var role in r)
            {             
                await _usermanager.AddToRoleAsync(user, role);
                                 
            }          
            //await _usermanager.AddToRolesAsync(user, );

            return RedirectToAction("Home", "Index");
        }

        [Authorize]
        public IActionResult MyCalendar()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetLocations());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetMyEvents(userid));
            ViewData["Hols"] = JSONListHelper.GetHolListJSONString(_idal.GetApprovedHolForUser(userid));
            return View();
        }

        [Authorize]
        public IActionResult UserList()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _idal.GetUserByAspNetId(userid);
            var userList = _idal.GetUsers(user.Location);
            return View(userList);
        }

        [Authorize]
        public async Task<IActionResult> UserDetails()
        {
            //var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var user = await _usermanager.GetUserAsync(User);
            //var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //return View(user);
            

            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }

            var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<SelectListItem> roles = PopulateRoles();


            foreach(SelectListItem item in roles)
            {
                if (_idal.isUserInRole(item.Text, user.Id)){
                    item.Selected = true;
                }
            }

            List<Holiday> holidays = _idal.GetHolidays(user.Id);

            return View(new UserDetailsVM(user,roles, User.FindFirstValue(ClaimTypes.NameIdentifier), holidays));
        }

        public List<SelectListItem> PopulateRoles()
        {
            var roles = _idal.GetRoles();

            List<SelectListItem> rolesList = new List<SelectListItem>();

            foreach (var role in roles)
            {
                rolesList.Add(new SelectListItem { Text = role.Name, Value = role.Id });
            }

            return rolesList;
        }

        public IActionResult OtherUserDetails(int id)
        {
            //var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _idal.GetUserByUserId(id);
            List<SelectListItem> roles = PopulateRoles();

            foreach (SelectListItem item in roles)
            {
                if (_idal.isUserInRole(item.Text, user.Id))
                {
                    item.Selected = true;
                }
            }

            List<Holiday> holidays = _idal.GetHolidays(user.Id);

            return View(new UserDetailsVM(user, roles, user.AspNetUser.Id, holidays));
            //return View(new UserDetailsVM(user, roles,user.AspNetUser.Id));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}