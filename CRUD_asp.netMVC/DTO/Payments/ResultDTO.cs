using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CRUD_asp.netMVC.DTO.Payment
{
    public class ResultDTO
    {
        public ResultDTO(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }

        public static ResultDTO OK(string message) => new ResultDTO(true, message);
        public static ResultDTO Fail(string message) => new ResultDTO(false, message);
    }
}
