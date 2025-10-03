using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Fluent;
using Repairly.Models;
using Repairly.Repository;
using System.Data;

namespace Repairly.Controllers
{
    public class ReportController : Controller
    {
      

        private readonly IReportRepository _repo;
        public ReportController(IReportRepository repo)
        {
            _repo = repo;
      

        }
        public IActionResult Index()
        {
            var data = _repo.GetAllData();
             
            var category = _repo.GetCategory();
         
            var categorySummary = data.GroupBy(x => x.category).ToDictionary(g => g.Key, g => g.Count());
             var brandSummary = data.GroupBy(x => x.brand).ToDictionary(g => g.Key, g => g.Count());
            var statusSummary = data.GroupBy(x => x.status).ToDictionary(g => g.Key, g => g.Count());
            ReportDataViewModel dataset = new();

            dataset.Data = data;
            dataset.StatusSummary = statusSummary;
            dataset.CategorySummary = categorySummary;
            dataset.BrandSummary = brandSummary;

            category.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "ประเภททั้งหมด"
            });
            ViewBag.Category = new SelectList(category, "Value", "Text");


            var statusList = _repo.GetStatus().ToList();
            statusList.Insert(0,new SelectListItem
            {   
                Text = "สถานะทั้งหมด",
                Value = "0"

            });
            ViewBag.Status = new SelectList(statusList, "Value", "Text");


            return View(dataset);
        }

        public IActionResult SearchReport(DateTime? startDate, DateTime? endDate, int Status, int Category)
        {
            var data = _repo.Search( startDate, endDate, Status, Category);
            var categorySummary = data.GroupBy(x => x.category).ToDictionary(g => g.Key, g => g.Count());
            var brandSummary = data.GroupBy(x => x.brand).ToDictionary(g => g.Key, g => g.Count());
            var statusSummary = data.GroupBy(x => x.status).ToDictionary(g => g.Key, g => g.Count());
            ReportDataViewModel dataset = new();
            dataset.Data = data;
            dataset.StatusSummary = statusSummary;
            dataset.CategorySummary = categorySummary;
            dataset.BrandSummary = brandSummary;
            return PartialView("TableReport", dataset);
        }
        public IActionResult ExportPdf(DateTime? startDate, DateTime? endDate, int status, int category)
        {
            var filtered = _repo.Search(startDate, endDate, status, category);

            var model = new ReportDataViewModel
            {
                Data = filtered,
                StatusSummary = filtered.GroupBy(x => x.status).ToDictionary(g => g.Key, g => g.Count()),
                CategorySummary = filtered.GroupBy(x => x.category).ToDictionary(g => g.Key, g => g.Count()),
                BrandSummary = filtered.GroupBy(x => x.brand).ToDictionary(g => g.Key, g => g.Count()),
                start_date = startDate.HasValue ? startDate.Value.ToString("dd/MM/yyyy") : "",
                end_date = endDate.HasValue ? endDate.Value.ToString("dd/MM/yyyy") : "",
            };
         
            var document = new RepairReportDocument(model);
            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream, "application/pdf", "รายงานการซ่อม.pdf");
        }



    }
}

