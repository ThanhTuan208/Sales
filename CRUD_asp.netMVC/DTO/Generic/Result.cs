    using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    namespace CRUD_asp.netMVC.DTO.Generic
    {
        public class Result<T>
        {
            public Result(bool success, string message, int statusCode, T? data)
            {
                Success = success;
                Message = message;
                StatusCode = statusCode;
                Data = data;
            }

            public bool Success { get; }
            public string Message { get; }
            public int StatusCode { get; set; }
            public T? Data{ get; set; }

            public static Result<T> OK(string message, int statusCode, T? data = default) => new Result<T>(true, message, statusCode, data);
            public static Result<T> Fail(string message, int statusCode, T? data = default) => new Result<T>(false, message, statusCode, data);
        }
    }
