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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ConnectCore_v2.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _usermanager;

        
        public LocationController(IDAL idal, UserManager<ApplicationUser> usermanager)
        {
            _dal = idal;
            _usermanager = usermanager;
        }

        // GET: Location
        public IActionResult Index()
        {
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            return View(_dal.GetLocations());
        }

        // GET: Location/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = _dal.GetLocation((int)id);
               
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Location location)
        {
            
                try
                {
                    _dal.CreateLocation(location);
                    TempData["Alert"] = "Success! You created a location for: " + location.Name;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Alert"] = "An error occurred: " + ex.Message;
                    return View(location);
                }

            
            return View(location);
        }


    }
}
