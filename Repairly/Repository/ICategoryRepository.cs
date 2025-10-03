using Repairly.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Repository
{
    public interface ICategoryRepository
    {

        List<CategoryViewModel> GetAllData();
        bool CreateCate(CategoryViewModel data);
        bool DeleteCate(int id);
        CategoryViewModel getId(int id);
        bool UpdateCate(CategoryViewModel data);
    }
}
