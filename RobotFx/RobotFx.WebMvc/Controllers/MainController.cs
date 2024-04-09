using Microsoft.AspNetCore.Mvc;
using RobotFx.Repositories.EntityFamework.Interface;
using RobotFx.WebMvc.MemCached.Interface;

namespace RobotFx.WebMvc.Controllers
{
    public class MainController : BaseController
    {
        public MainController(IMemCached memCached, IUnitOfWork unitOfWork) 
            : base(memCached, unitOfWork)
        {
        }

        public IActionResult Menu()
        {
            return View();
        }
    }
}
