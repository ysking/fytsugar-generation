using System;
using System.Net;

namespace FytSugar.Builder
{
    public class JResult<Object>
    {
        public static ApiResult<Object> Error(string errMsg)
        {
            return new ApiResult<Object>
            {
                Code = -1,
                Message = errMsg
            };
        }

        public static ApiResult<Object> Error()
        {
            return new ApiResult<Object>
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = "服务端发生错误"
            };
        }

        public static ApiResult<Object> Success()
        {
            return new ApiResult<Object>
            {
                Code = (int)HttpStatusCode.OK
            };
        }

        public static ApiResult<object> Success(Object resultData)
        {
            return new ApiResult<object>
            {
                Code = (int)HttpStatusCode.OK,
                Data = resultData
            };
        }
    }

    /// <summary>
    /// API 返回JSON格式
    /// </summary>
    public class ApiResult<Object>
    {
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = (int)HttpStatusCode.InternalServerError;

        /// <summary>
        /// 数据集
        /// </summary>
        public Object Data { get; set; }
    }
}
