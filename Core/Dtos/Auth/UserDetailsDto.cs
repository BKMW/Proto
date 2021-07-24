﻿using System;

namespace Application.Dtos.Auth
{
    public class UserDetailsResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastConnection { get; set; }


    }
}
