using API6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Test.Fixtures
{
    public static class UsersFixture
    {
        public static List<User> Getusers() => new List<User>
        {
            new User()
            {
                Id = 1,
                Name = "User1",
                Email = "Jane@gmail.com",
                Address = new Address()
                {
                    Street = "Manhatan",
                    City = "New York",
                    ZipCode = "0450"

                }
            },
             new User()
            {
                Id = 2,
                Name = "User2",
                Email = "Jane@gmail.com",
                Address = new Address()
                {
                    Street = "Manhatan",
                    City = "New York",
                    ZipCode = "0450"

                }
            },
              new User()
            {
                Id = 3,
                Name = "User3",
                Email = "Jane@gmail.com",
                Address = new Address()
                {
                    Street = "Manhatan",
                    City = "New York",
                    ZipCode = "0450"

                }
            }
        };
    
    }
}
