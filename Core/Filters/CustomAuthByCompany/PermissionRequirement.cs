using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filters.CustomAuthByCompany
{
    public class CompanyRequirement : IAuthorizationRequirement
    {
        public string _company { get; private set; }

        public CompanyRequirement(string company)
        {
            _company = company;
        }
    }
}