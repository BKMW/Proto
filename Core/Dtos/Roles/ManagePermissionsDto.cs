using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Roles
{
    public class ManagePermissionsRequest
    {
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public List<string> RoleCalims { get; set; }
    }
}
