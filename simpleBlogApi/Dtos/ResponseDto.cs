using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos;

namespace simpleBlogApi.Dtos
{
    //? generic class T can be any object like User, List<Product>, object...
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ResponseDto(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
