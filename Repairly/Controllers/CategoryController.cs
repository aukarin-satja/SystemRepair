using Microsoft.AspNetCore.Mvc;
using Repairly.Models;
using Repairly.Repository;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;
        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            var cate = _repo.GetAllData();
            return View(cate);
        }

        public IActionResult CreateCate(CategoryViewModel data)
        {
            if (_repo.CreateCate(data))
            {
               return RedirectToAction("Index");

            }
            ViewBag.ErrorMessage = "ข้อมูลนี้มีอยู่แล้ว";
            return View("Index");
        }

        public IActionResult Delete(int id)
        {
            if (_repo.DeleteCate(id))
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        public JsonResult GetId(int id)
        {
            var cate = _repo.getId(id);

            return Json(new
            {
                id = cate.id,
                name = cate.name

            });
        }

        public IActionResult UpdateCate(CategoryViewModel data)
        {
            if (_repo.UpdateCate(data))
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }
    }
}
