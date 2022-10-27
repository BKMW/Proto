using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        string GetUserId { get; }
        string GetUserName { get; }
        string GetRole { get; }
        string GetJti { get; }

    }
}
