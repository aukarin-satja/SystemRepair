using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Repository
{
    public interface IAssetsRepository
    {
        List<AssetsViewModel> GetAssets(int page, int pagesize, out int totalRecord);
         List<SelectListItem> GetItem();
        bool CreateAsset(AssetsPageViewModel model);
        FormAsset GetAssetById(int id);
        bool UpdateAsset(AssetsPageViewModel data);
        bool DeleteAsset(int id);
        List<AssetsViewModel> SearchAsset(string data);
    }
}
