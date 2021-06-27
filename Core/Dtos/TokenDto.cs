using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class TokenRequest
    {
        [Required]
        public string token { get; set; }
        [Required]
        public string refreshToken { get; set; }
    }
    public class TokenResponse
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public int resultCode { get; set; }
    }
}
