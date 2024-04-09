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
