using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Common.Models
{
    public abstract class BaseResult
    {
        protected BaseResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
    }

    public class ResultData<T> : BaseResult
    {
        public ResultData(T? data, bool isSuccess = true, string message = "") : base(isSuccess, message) 
        {
            Data = data;
        }
        public T? Data { get; private set; }

        public static ResultData<T> Success(T data) => new(data);
        public static ResultData<T> Error(string message) => new(default, false, message);
    }

}
