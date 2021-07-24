using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public class LoginRequest
    {
        
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }

      
    }
    public class LoginResponse
    {

        public string userName { get; set; }
        public string role { get; set; }
        public string Company { get; set; }
        public DateTime? lastConnection { get; set; }
     



    }
}
