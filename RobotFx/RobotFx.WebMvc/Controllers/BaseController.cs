using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RobotFx.DoMain.Models.BaseModel;
using RobotFx.Repositories.EntityFamework.Interface;
using RobotFx.WebMvc.MemCached.Interface;
using System.Net;

namespace RobotFx.WebMvc.Controllers
{
    public class BaseController : Controller, IAsyncActionFilter
    {
        protected readonly IMemCached _memCached;
        protected readonly IUnitOfWork _unitOfWork;
        public BaseController(IMemCached memCached, IUnitOfWork unitOfWork)
        {
            _memCached = memCached;
            _unitOfWork = unitOfWork;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _memCached.Dispose();
                _unitOfWork.Dispose();
                //((IDisposable)provider).Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userData = _memCached.GetCurrentUser();
            var controller = context.Controller as Controller;
            if (!context.HttpContext.Request.Path.Equals("/AccountFx/GetInfoById") && controller != null && userData == null && !context.HttpContext.Request.Path.Equals("/Account/Login") && !context.HttpContext.Request.Path.Equals("/Account/ExecuteLogin"))
            {
                if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // Xử lý chuyển hướng Ajax bằng cách trả về một phản hồi JSON
                    context.Result = new JsonResult(new { redirectTo = "/Account/Login" });
                }
                else
                {
                    // Xử lý chuyển hướng thông thường
                    context.Result = new RedirectResult("/Account/Login");
                }
                return;
            }

            // Thực thi action
            var resultContext = await next();
            // Đoạn code sau khi action được thực thi
        }

        protected DataResponse<TRequest> Success_Request<TRequest>(TRequest data)
        {
            return new DataResponse<TRequest>()
            {
                Data = data,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Thành công"
            };
        }

        protected StatusResponse Success_Request()
        {
            return new StatusResponse()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Thành công"
            };
        }

        protected StatusResponse Not_Found(string message = "")
        {
            return new StatusResponse()
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = string.IsNullOrEmpty(message) ? "Không tìm thấy dữ liệu" : message
            };
        }

        protected StatusResponse Bad_Request(string message = "")
        {
            return new StatusResponse()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = string.IsNullOrEmpty(message) ? "Dữ liệu định dạng sai" : message
            };
        }

        protected StatusResponse Server_Error(string message = "")
        {
            return new StatusResponse()
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = string.IsNullOrEmpty(message) ? "Có lỗi trong quá trình xử lý" : message
            };
        }
    }
}
