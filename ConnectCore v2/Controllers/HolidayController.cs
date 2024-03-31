using ConnectCore_v2.Data;
using ConnectCore_v2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectCore_v2.Controllers
{
    public class HolidayController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public HolidayController(IDAL dal, UserManager<ApplicationUser> usermanager)
        {
            _dal = dal;
            _usermanager = usermanager;
        }


        public IActionResult Index()
        {
            List<Holiday> hols = _dal.GetHolidaysForApproval();
            return View(hols);
        }

        public IActionResult ApprovedHols()
        {
            List<Holiday> hols = _dal.GetApprovedHolidays();
            return View(hols);
        }

        public IActionResult PopupApproveHol(int id)
        {
            Holiday hol = _dal.GetholById(id);

            return PartialView(hol);
        }

        [HttpPost]
        public IActionResult ApproveHolidays(int id)
        {
        
            var user = _dal.GetUserById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Event> userShifts = _dal.GetUsersFutureEvents(user.Id);
            Holiday hol = _dal.GetholById(id);

            ////check if any overlapping shifts with the approved hol and remove 
            foreach (var shift in userShifts)
            {
                if( (hol.StartTime.Date >= shift.StartTime.Date && hol.StartTime.Date <= shift.StartTime.Date) || (hol.EndTime.Date >= shift.EndTime.Date && hol.EndTime.Date <= shift.EndTime.Date))
                {
                    _dal.removeEvent(shift);
                }
            }
         
            _dal.ApproveHoliday(hol,user);
          
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CancelHolidays(IFormCollection form)
        {
            var hols = form["HolIds"].ToList();
            var holList = _dal.GetHolsById(hols);

            _dal.CancelHolidays(holList);

            return RedirectToAction("ApprovedHols");
        }

        
        public IActionResult CancelHoliday(int Id)
        {
            
            var hol = _dal.GetholById(Id);

            _dal.CancelHoliday(hol);

            return RedirectToAction("Home", "UserDetails");
        }


    }
}
