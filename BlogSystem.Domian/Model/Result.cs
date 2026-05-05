using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domian.Model
{
    public class Result<T>
    {
        public T? Value { get; }
        public string? ErrorMessage { get; set; }
        public bool? IsSuccess { get; }

        private Result(T? value, string? errorMessage)
        {
            Value = value;
            ErrorMessage = errorMessage;
            IsSuccess = errorMessage == null;

        }

        public static Result<T> Success(T value) => new Result<T>(value, null);
        public static Result<T> Failure(string errorMessage) => new Result<T>(default, errorMessage);

        public static Result<T> Success() => new Result<T>(default, null);
    }
}
