using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.Auth
{
    public class UpdateProfileRequest
    {
      
        //[MinLength(8)]
        public string PhoneNumber { get; set; }
        //[Required]
        public string FirstName { get; set; }
       // [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
