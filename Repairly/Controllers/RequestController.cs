using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;
using Repairly.Repository;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestRepository _repo;
        public RequestController(IRequestRepository config)
        {
            _repo = config;
        }

        public IActionResult Index()
        {
            var request = _repo.GetAllData();

            var statusList = _repo.GetStatus().ToList(); 
            statusList.Insert(0, new SelectListItem { Value = "0", Text = "สถานะทั้งหมด" }); 
            ViewBag.Status = new SelectList(statusList, "Value", "Text");

            var CategoryList = _repo.GetCategory().ToList();
            CategoryList.Insert(0, new SelectListItem { Value = "0", Text = "ประเภททั้งหมด" });
            ViewBag.Cateogory = new SelectList(CategoryList, "Value", "Text");

         
            return View(request);
        }


        public JsonResult SearchAsset(string data)
        {
            var assets = _repo.SearchAsset(data); // คืน List<AssetViewModel>

            var result = assets.Select(a => new
            {
                id = a.id,
                name = a.name,
                assets_code = a.asset_code,
                model =a.model,
                brand = a.brand,
                category_id = a.category_id,
                category = a.category
,

            });

            return Json(result);
        }

        public JsonResult SearchUser(string data)
        {
            var user = _repo.GetUser(data);

            var result = user.Select(i => new
            {
                user_id = i.user_id,
                user_name = i.user_name,
                email = i.email
            });


            return Json(result);
        }

        public IActionResult CreateRequest(SelectItemViewModel data)
        {
            if (_repo.CreatRequest(data))
            {
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (_repo.DelRequest(id))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        public JsonResult GetRequestDetail(int id)
        {
            var result = _repo.GetDetail(id);

            return Json(new
            {
                id = result.id,
                name = result.name,
                user_name = result.user_name,
                email = result.email,
                category = result.category,
                description = result.description,
                status_id = result.status_id,
                brand = result.brand,
                model = result.model,
            });
        }

        public IActionResult UpdateRequest(SelectItemViewModel data)
        {
            if (_repo.UpdateRequest(data))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult SearchRequest(string keyword, int status, int category)
        {

            var req = _repo.SearchRequest(keyword, status, category);
            return PartialView("TableRequest", req);
        }

    }
}
