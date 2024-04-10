using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RobotFx.DoMain.FileLog;
using RobotFx.DoMain.Models;
using RobotFx.Repositories.EntityFamework;
using RobotFx.Repositories.EntityFamework.Interface;
using RobotFx.WebMvc.MemCached.Interface;
using RobotFx.WebMvc.Models;
using System.Linq;
using System.Reflection;

namespace RobotFx.WebMvc.Controllers
{
    public class AccountController : BaseController
    {      
        private readonly IRepository<User> _userRepositor;

        public AccountController(IMemCached memCached, IUnitOfWork unitOfWork, IRepository<User> userRepository) 
            : base(memCached, unitOfWork)
        {
            _userRepositor = userRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecuteLogin(string loginViewModelJson, int screenWidth, int screenHeight)
        {
            try
            {
                var loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(loginViewModelJson);
                var users = _userRepositor.AsQueryable().
                    Where(x => x.Account.ToLower() == loginViewModel.LoginName && x.Password == loginViewModel.Password.ToLower() && !x.IsDeleted)
                    .FirstOrDefault();
                if (users != null)
                {
                    if (!loginViewModel.IsSaveCookies || users.ExpireDate.Date < DateTime.Now.Date)
                    {
                        loginViewModel.IsSaveCookies = false;
                        loginViewModel.Password = string.Empty;
                        loginViewModel.Imei = string.Empty;
                    }
                    _memCached.ExecuteSaveUserPassword(loginViewModel);
                    _memCached.ExecuteSaveData(users);
                    int remainingDays = (users.ExpireDate.Date - DateTime.Now.Date).Days;
                    return Json(Success_Request(remainingDays));
                }
                else
                {
                    return Json(Server_Error("Tài khoản đăng nhập không đúng!"));
                }
            }
            catch (Exception ex)
            {
                FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Json(Server_Error("Hệ thống đang xảy ra lỗi!"));
            }
        }

        private string CreateImeiByDevice(int screenWidth, int screenHeight)
        {
            string imei = string.Empty;
            string userAgent = HttpContext.Request.Headers["User-Agent"].ToString().ToLower();
            if (userAgent.Contains("iphone") && userAgent.Contains("mobile"))
            {
                imei = string.Format("Mobile-Iphone-ScreenWidth={0}-ScreenHeight={1}", screenWidth, screenHeight);
            }
            else if (userAgent.Contains("android") && userAgent.Contains("mobile"))
            {
                imei = string.Format("Mobile-Android-ScreenWidth={0}-ScreenHeight={1}", screenWidth, screenHeight);
            }
            else if (userAgent.Contains("windows"))
            {
                imei = string.Format("Computer-Windows-ScreenWidth={0}-ScreenHeight={1}", screenWidth, screenHeight);
            }
            else if ((userAgent.Contains("ipad") && userAgent.Contains("mac os")) || (userAgent.Contains("macintosh") && userAgent.Contains("mac os")))
            {
                imei = string.Format("Mobile-Ipad-Os-ScreenWidth={0}-ScreenHeight={1}", screenWidth, screenHeight);
            }
            else
            {
                imei = string.Format("Unknown-Device-ScreenWidth={0}-ScreenHeight={1}", screenWidth, screenHeight);
            }

            return imei;
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
