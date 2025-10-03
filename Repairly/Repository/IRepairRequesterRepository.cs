using Repairly.Models;

namespace Repairly.Repository
{
    public interface IRepairRequesterRepository
    {
        List<RepairRequestViewModel> GetRequester();
      
    }
}
