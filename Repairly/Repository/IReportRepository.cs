using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;

namespace Repairly.Repository
{
    public interface IReportRepository
    {
        List<ReportViewModel> GetAllData();
        List<SelectListItem> GetStatus();
        List<SelectListItem> GetCategory();
        List<ReportViewModel> Search(DateTime? startDate, DateTime? endDate, int status, int Category);

    }
}
