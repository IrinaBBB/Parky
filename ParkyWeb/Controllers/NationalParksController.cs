using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.IO;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _repo;

        public NationalParksController(INationalParkRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();
            if (id == null)
            {
                return View(obj);
            }

            obj = await _repo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());


            if (obj == null) return NotFound();
            else return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Picture = p1;
                } else
                {
                    if (obj.Id != 0)
                    {
                        var objFromDb = await _repo.GetAsync(SD.NationalParkAPIPath, obj.Id);
                        obj.Picture = objFromDb.Picture;
                    }
                }
                if (obj.Id == 0)
                {
                    await _repo.CreateAsync(SD.NationalParkAPIPath, obj);
                }
                else
                {
                    await _repo.UpdateAsync(SD.NationalParkAPIPath + obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repo.DeleteAsync(SD.NationalParkAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            return Json(new { success = false, message = "Not Deleted" });

        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { data = await _repo.GetAllAsync(SD.NationalParkAPIPath) });
        }

    }
}
