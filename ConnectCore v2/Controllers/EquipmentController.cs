using ConnectCore_v2.Data;
using ConnectCore_v2.Helpers;
using ConnectCore_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectCore_v2.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IDAL _idal;

    public EquipmentController(IDAL idal)
        {
            _idal = idal;
        }

        public IActionResult Index()
        {
            var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            List<Equipment> equipment = _idal.GetEquipment(user.Location);

            return View(equipment);
        }

        public IActionResult EquipmentSchedule()
        {
            var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewData["Equipment"] = JSONListHelper.GetEquipmentListJSONString(_idal.GetEquipmentBooked(user.Location));

            return View();
        }

        public IActionResult CreateEquipment()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEquipment(IFormCollection form)
        {

            try
            {
                var user = _idal.GetUserByAspNetId(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _idal.CreateEquipment(form, user.Location);
               return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
               return RedirectToAction("Index");
            }

            
        }
    }
}
