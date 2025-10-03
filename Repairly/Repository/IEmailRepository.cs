using Repairly.Models;

namespace Repairly.Repository
{
    public interface IEmailRepository
    {

        List<EmailViewModel> GetAllData();
    }
}
