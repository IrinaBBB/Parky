using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _tRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository tRepo)
        {
            _npRepo = npRepo;
            _tRepo = tRepo;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            TrailsViewModel objVM = new TrailsViewModel
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                return View(objVM);
            }

            objVM.Trail = await _tRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());

            if (objVM.Trail == null) return NotFound();
            else return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Trail.Id == 0)
                {
                    await _tRepo.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _tRepo.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction(nameof(Index));
            } else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
                obj = new TrailsViewModel
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _tRepo.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            return Json(new { success = false, message = "Not Deleted" });

        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _tRepo.GetAllAsync(SD.TrailAPIPath) });
        }
    }
}
