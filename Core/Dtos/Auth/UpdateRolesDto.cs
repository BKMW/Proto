using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.Auth
{
    public class UpdateRolesRequest
    {
        public string Id { get; set; }
        public List<string> roles { get; set; }

    }
}
