using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;

namespace Repairly.Repository
{
    public interface IRequestRepository
    {
        List<RequestViewModel> GetAllData();
        List<SelectItemViewModel> SearchAsset(string data);
        List<SelectItemViewModel> GetUser(string data);
        bool CreatRequest(SelectItemViewModel data);
        bool DelRequest(int id);
        List<SelectListItem> GetStatus();
        SelectItemViewModel GetDetail(int id);
        bool UpdateRequest(SelectItemViewModel id);
        List<RequestViewModel> SearchRequest(string keyword, int status, int category);
        List<SelectListItem> GetCategory();
    }
}
