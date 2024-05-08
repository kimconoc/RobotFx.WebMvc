using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RobotFx.DoMain.Enum;
using RobotFx.DoMain.FileLog;
using RobotFx.DoMain.Models;
using RobotFx.DoMain.Models.ModelsResponse;
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
            var userData = _memCached.GetCurrentUser();
            var listAccountFx = _accountFxRepositor.AsQueryable().Where(x => x.UserId == userData.Id && !x.IsDeleted).ToList();
            return View(listAccountFx);
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
                        return Json(Bad_Request());
                }
                else
                {
                    if (!_accountFxRepositor.Update(accountFxs[0]))
                        return Json(Bad_Request());
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

        [HttpPost]
        public IActionResult ExecuteDeleteAccountFx(int accountFxId)
        {
            try
            {
                var userData = _memCached.GetCurrentUser();
                var accountFx = _accountFxRepositor.AsQueryable().Where(x => x.UserId == userData.Id && x.Id == accountFxId).FirstOrDefault();

                if (!_accountFxRepositor.SoftDelete(accountFx))
                    return Json(Bad_Request());

                _unitOfWork.Commit();
                return Json(Success_Request());
            }
            catch(Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }
           
            return View(Server_Error());
        }

        [HttpGet]
        public IActionResult GetInfoById(string id)
        {
            if(string.IsNullOrEmpty(id))
                return Json(Bad_Request());

            var infoFx = _accountFxRepositor.AsQueryable().Where(x => x.IdAccountFx == id && !x.IsDeleted).FirstOrDefault();
            if(infoFx == null)
                return Json(Not_Found());

            AccountFxResponse accountFxResponse = new AccountFxResponse()
            {
                AccountNumber = infoFx.IdAccountFx,
                SignalFlag = infoFx.SignalType == (int)SignalTypeEnum.random ? false : true,
                Signal = infoFx.SignalType == (int)SignalTypeEnum.random ? 0 : infoFx.SignalType,
                AutoTrading = infoFx.IsOnline == (int)IsOnlineTypeEnum.Off ? false : true,
            };

            return Json(accountFxResponse);
        }

    }
}
