using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;
using Repairly.Repository;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Controllers
{
    public class AssetsController : Controller
    {

        private readonly IAssetsRepository _repo;
        public AssetsController(IAssetsRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index(int page =1)
        {
           
            int pagsize = 10;
            int Record;
            var asset = _repo.GetAssets(page, pagsize,out Record);
 

            AssetsPageViewModel assets = new()
            {
                Assets = asset,
                CurrentPage = page,
                PageSize= pagsize,
                TotalRecord= Record,
             

            };
            ViewBag.Categories = new SelectList(
                    _repo.GetItem(), "Value", "Text"
             );

            
            

            return View(assets);
        }
        public IActionResult Popupcard()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateAsset(AssetsPageViewModel model)
        {

          
            if (_repo.CreateAsset(model))
            {
                return RedirectToAction("Index");
            }



            return View();
        }


        public IActionResult GetAssetDetail(int id)
        {
            var asset = _repo.GetAssetById(id); 
            return Json(new
            {
                id = asset.Assets_id,
                code = asset.Asset_code,
                name = asset.Asset_name,
                category = asset.Category_id,
                brand = asset.Brand,
                model = asset.Model,
                location = asset.Location,
                purchaseDate = asset.Purchase_date.ToString("yyyy-MM-dd"),
                warrantyDate = asset.Warranty_date.ToString("yyyy-MM-dd")
            });
        }

        public IActionResult UpdateAsset(AssetsPageViewModel data)
        {
            if (_repo.UpdateAsset(data))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (_repo.DeleteAsset(id))
            {
                return RedirectToAction("Index");
                TempData["SuccessMessage"] = "ลบรายการสำเร็จแล้ว";

            }
            return RedirectToAction("Index");

        }

        public IActionResult SearchAssets(string data)
        {

            if (string.IsNullOrWhiteSpace(data))
            {
                return Index(1);
            }
            else
            {
                var asset = _repo.SearchAsset(data);
                AssetsPageViewModel assets = new()
                {
                    Assets = asset
                };

                return PartialView("AssetsTablePartial", assets);

            }



        }

    }
}
