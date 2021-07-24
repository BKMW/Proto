using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Roles
{
    public class AddRoleRequest
    {
        [Required, StringLength(30)]
        public string Name { get; set; }
    }
}
