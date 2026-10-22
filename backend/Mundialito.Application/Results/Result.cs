using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }

        public static Result Success()
        {
            return new Result
            {
                IsSuccess = true
            };
        }

        public static Result Failure(string errorMessage, string errorCode)
        {
            return new Result
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
