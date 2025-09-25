using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service.Helper
{
    public class PagedApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        
        public PagedApiResponse(IEnumerable<T> data, int totalItems, int page, int pageSize, string? message = null)
        {
            Success = true;
            Data = data;
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
            Message = message;
        }

        public PagedApiResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }

}
