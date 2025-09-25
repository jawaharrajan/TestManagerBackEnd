﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service.Helper
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        //public IEnumerable<T> DataList { get; set; }

        public ApiResponse(T data, string? message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }

        //public ApiResponse(IEnumerable<T> data, string? message = null)
        //{
        //    Success = true;
        //    Data = data;
        //    Message = message;
        //}

        public ApiResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }
    }

}
