using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.Identity.Entities
{
    public class User : IdentityUser
    {
        public DateTime? LastConnection { get; set; }
        public DateTime? ResetPassword { get; set; }
        public DateTime? Addeddate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
       // public virtual IList<RefreshToken> RefreshTokens { get; set; }

    }
}
