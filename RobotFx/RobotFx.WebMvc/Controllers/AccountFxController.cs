using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RobotFx.DoMain.FileLog;
using RobotFx.DoMain.Models;
using RobotFx.WebMvc.MemCached.Interface;
using System.Reflection;

namespace RobotFx.WebMvc.Controllers
{
    public class AccountFxController : BaseController
    {
        public AccountFxController(IMemCached memCached) : base(memCached)
        {
        }

        public IActionResult ListAccountFx()
        {
            return View();
        }
        //public IActionResult ExecutionAccountFx(int? playerId)
        //{
        //    return View(Server_Error());
        //}

        //[HttpPost]
        //public IActionResult ExecuteAccountFx(string playerJsons)
        //{
        //    return View(Server_Error());
        //}

        //[HttpPost]
        //public IActionResult ExecuteDeleteAccountFx(int playerId)
        //{
        //    return View(Server_Error());
        //}
    }
}
