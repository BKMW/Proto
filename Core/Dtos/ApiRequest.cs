using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class ApiRequest
    {
        [Required]
        public string token { get; set; }
        [Required]
        public string refreshToken { get; set; }
    }

    public class ApiRequest<T> : ApiResponse
    {
        public T data { get; set; }
    }
   
}
