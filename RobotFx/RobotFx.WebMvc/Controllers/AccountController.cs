using Microsoft.AspNetCore.Mvc;
using RobotFx.DoMain.FileLog;
using System.Reflection;

namespace RobotFx.WebMvc.Controllers
{
    public class AccountController : BaseController
    {
       
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExecuteLogin(string loginViewModelJson, int screenWidth, int screenHeight)
        {
            try
            {
                return Json(Success_Request("oke"));
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
