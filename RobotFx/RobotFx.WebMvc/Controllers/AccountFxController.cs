using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RobotFx.DoMain.FileLog;
using RobotFx.DoMain.Models;
using RobotFx.Repositories.EntityFamework;
using RobotFx.Repositories.EntityFamework.Interface;
using RobotFx.WebMvc.MemCached.Interface;
using System.Linq;
using System.Reflection;

namespace RobotFx.WebMvc.Controllers
{
    public class AccountFxController : BaseController
    {
        private readonly IRepository<AccountFx> _accountFxRepositor;
        public AccountFxController(IMemCached memCached, IUnitOfWork unitOfWork, IRepository<AccountFx> accountFxRepositor) 
            : base(memCached, unitOfWork)
        {
            _accountFxRepositor = accountFxRepositor;
        }

        public IActionResult ListAccountFx()
        {
            return View();
        }

        public IActionResult ExecutionAccountFx(int? accountFxId)
        {
            AccountFx accountFx = new AccountFx();
            if (accountFxId != null)
            {
                accountFx = _accountFxRepositor.AsQueryable().Where(x => x.Id == accountFxId).FirstOrDefault();
                if(accountFx == null)
                    accountFx = new AccountFx();
            }

            return View(accountFx);
        }

        [HttpPost]
        public IActionResult ExecuteAccountFx(string accountFxJsons)
        {
            try
            {
                var userData = _memCached.GetCurrentUser();
                var accountFxs = JsonConvert.DeserializeObject<List<AccountFx>>(accountFxJsons);
                accountFxs[0].UserId = userData.Id;
                // Tạo mới and cập nhật
                if(accountFxs[0].Id == 0)
                {
                    if(!_accountFxRepositor.Insert(accountFxs[0]))
                        Json(Bad_Request());
                }
                else
                {
                    if (!_accountFxRepositor.Update(accountFxs[0]))
                        Json(Bad_Request());
                }

                _unitOfWork.Commit();
                return Json(Success_Request());
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }

            return View(Server_Error());

        }

    }
}
