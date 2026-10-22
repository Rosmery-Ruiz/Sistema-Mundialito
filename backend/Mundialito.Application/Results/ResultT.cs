using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Results
{
    public class Result<T> : Result
    {
        public T? Value { get; set; }

        public static Result<T> Success(T value)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Value = value  /* se usa value porque siempre succes es de exito siempre debe devolver el valor obtenido*/
            };
        }

        public new static Result<T> Failure(string errorMessage, string errorCode)
        {
            return new Result<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }

    }

}
