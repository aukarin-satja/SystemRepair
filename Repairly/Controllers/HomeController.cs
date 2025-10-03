using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repairly.Models;
using Repairly.Repository;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Repairly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRepairRequesterRepository _repo;

     
        public HomeController(ILogger<HomeController> logger, IRepairRequesterRepository repo)
        {
            _logger = logger;
            _repo = repo; ;
        }



        public IActionResult Index()
        {
            List<RepairRequestViewModel> request = _repo.GetRequester();
            
            var groupedByType = request.GroupBy(r => r.Type).Select(group => new {
                TypeName = group.Key,
                TotalRequests = group.Count()
            }).ToList();
            var sortedRequest = request.OrderByDescending(r => r.CreatedAt).ToList();


            var monthData = Enumerable.Range(1, 12)
              .Select(m => new {
                  Month = m,
                  MonthName = new DateTime(2025, m, 1).ToString("MMM", new CultureInfo("th-TH")),
                  Count = request.Count(r => r.CreatedAt.Month == m)
              }).ToList();

            var totalActive = request.Count(r => r.StatusName == "กำลังดำเนินการ");
            var totalWait = request.Count(r => r.StatusName == "รอดำเนินการ");
            var totalSuccess = request.Count(r => r.StatusName == "เสร็จสิ้น");



            var vm = new DashboardViewModel
            {
                Requests = sortedRequest.Take(10).ToList(),
                ChartLabels = groupedByType.Select(x => x.TypeName).ToList(),
                ChartData = groupedByType.Select(x => x.TotalRequests).ToList(),
                MonthsLabels = monthData.Select(x => x.MonthName).ToList(),
                MonthsData = monthData.Select(x => x.Count).ToList(),
                Active = totalActive,
                Wait = totalWait,
                Success = totalSuccess,
            };


            
            
            

            return View(vm);
        }






        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
