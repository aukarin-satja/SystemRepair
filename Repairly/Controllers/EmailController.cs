using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Repairly.Repository;

namespace Repairly.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailRepository _repo;
        public EmailController (IEmailRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {

            var data = _repo.GetAllData();
            return View(data);
        }
    }
}
