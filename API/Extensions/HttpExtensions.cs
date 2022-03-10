using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddHeader(this HttpResponse response,int currentPage,int itemsPerPage,int totalItems,int totalPages)
        {
            var header = new 
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };
            response.Headers.Add("Pagination",JsonSerializer.Serialize(header));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}