using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class ClaimsCurrentUser
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Jti { get; set; }

    }
}
