using Microsoft.AspNetCore.Mvc;
using RobotFx.WebMvc.MemCached.Interface;

namespace RobotFx.WebMvc.Controllers
{
    public class MainController : BaseController
    {
        public MainController(IMemCached memCached) : base(memCached)
        {
        }

        public IActionResult Menu()
        {
            return View();
        }
    }
}
