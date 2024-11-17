using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNetCoreCalendar.Data;
using DotNetCoreCalendar.Models;

namespace DotNetCoreCalendar.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly IDAL _dal;

        public LocationController(IDAL idal)
        {
            _dal = idal;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Location location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dal.CreateLocation(location);
                    TempData["Alert"] = $"Success! You created a location: {location.Name}";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Alert"] = $"An error occurred: {ex.Message}";
                }
            }
            return View(location);
        }

        // GET: Location/Edit/5
        public IActionResult Edit(int? id)
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

        // POST: Location/Edit/5
        // POST: Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Отримання існуючого запису
                    var existingLocation = _dal.GetLocation(id);
                    if (existingLocation == null)
                    {
                        return NotFound();
                    }

                    // Оновлення даних
                    existingLocation.Name = location.Name;

                    // Збереження оновленого об'єкта
                    _dal.UpdateLocation(existingLocation);

                    TempData["Alert"] = $"Success! You edited the location: {location.Name}";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Alert"] = $"An error occurred: {ex.Message}";
                }
            }
            return View(location);
        }
    

        // GET: Location/Delete/5
        public IActionResult Delete(int? id)
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

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var location = _dal.GetLocation(id);
                if (location == null)
                {
                    return NotFound();
                }

                _dal.DeleteLocation(id); // Додайте цей метод у IDAL та DAL
                TempData["Alert"] = $"Success! You deleted the location.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
