using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CRUD_asp.netMVC.DTO.Generic
{
    public class Result<T>
    {
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }

        public static Result<T> OK(string message) => new Result<T>(true, message);
        public static Result<T> Fail(string message) => new Result<T>(false, message);
    }
}
