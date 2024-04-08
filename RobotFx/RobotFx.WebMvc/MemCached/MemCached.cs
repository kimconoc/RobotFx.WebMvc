﻿using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using RobotFx.DoMain.Constant;
using RobotFx.DoMain.Enum;
using RobotFx.DoMain.Models;
using RobotFx.WebMvc.MemCached.Interface;
using RobotFx.WebMvc.Models;
using System.Runtime.InteropServices;

namespace RobotFx.WebMvc.MemCached
{
    public class MemCached : IMemCached
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemCached(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // To detect redundant calls
        private bool _disposedValue;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void ExecuteSaveData(User userData)
        {
            userData.UserAgent = IdentifyUserAgent();
            ExecuteSaveSession(userData);
        }
        public void RemoveSavedData()
        {
            //string value = string.Empty;
            //CookieOptions options = new CookieOptions
            //{
            //    Expires = DateTime.Now.AddDays(-1)
            //};
            //_httpContextAccessor.HttpContext.Response.Cookies.Append(GetSigninToken(), value, options);
            _httpContextAccessor.HttpContext.Session.Remove(GetSigninToken());
        }

        public User GetCurrentUser()
        {
            User user = null;
            try
            {
                string jsonValue = _httpContextAccessor.HttpContext.Session.GetString(GetSigninToken());
                if (!string.IsNullOrEmpty(jsonValue))
                {
                    user = JsonConvert.DeserializeObject<User>(jsonValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        public LoginViewModel GetCurrentUserPassword()
        {
            LoginViewModel loginViewModel = null;
            try
            {
                string jsonValue = _httpContextAccessor.HttpContext.Request.Cookies[GetSigninToken()];
                if (!string.IsNullOrEmpty(jsonValue))
                {
                    loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(jsonValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loginViewModel;
        }

        public void ExecuteSaveUserPassword(LoginViewModel loginViewModel)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                HttpOnly = true,
            };
            string jsonLoginViewModel = JsonConvert.SerializeObject(loginViewModel);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(GetSigninToken(), jsonLoginViewModel, cookieOptions);
        }

        private void ExecuteSaveSession(User userData)
        {
            string jsonUserData = JsonConvert.SerializeObject(userData);
            _httpContextAccessor.HttpContext.Session.SetString(GetSigninToken(), jsonUserData);
        }

        private string GetSigninToken()
        {
            return Default.Get_Signin_Token;
        }

        private int IdentifyUserAgent()
        {
            string userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString().ToLower();
            if (userAgent.Contains("iphone") && userAgent.Contains("mobile"))
            {
                return (int)UserAgentEnum.MobileIphone;
            }
            else if (userAgent.Contains("android") && userAgent.Contains("mobile"))
            {
                return (int)UserAgentEnum.MobileAndroid;
            }
            else if (userAgent.Contains("windows"))
            {
                return (int)UserAgentEnum.ComputerWindows;
            }
            else if ((userAgent.Contains("ipad") && userAgent.Contains("mac os")) || (userAgent.Contains("macintosh") && userAgent.Contains("mac os")))
            {
                return (int)UserAgentEnum.IpadOs;
            }
            else
            {
                return (int)UserAgentEnum.UnknownDevice;
            }
        }
    }
}
