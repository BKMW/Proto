using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.Auth
{
    public class UserListRequest
    {

        public string page { get; set; }
        public string size { get; set; }
        public string bank { get; set; }
        public string role { get; set; }
        public string userName { get; set; }

    }
    public class UserListResponse
    {

        public string Id { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public bool IsFirstConnection { get; set; }
        public DateTime? LastConnection { get; set; }
        public DateTime? ResetPassword { get; set; }
        public DateTime? AddedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }

    }
    public class UserListInfoResponse
    {
        public int countAll { get; set; }

    }

}
