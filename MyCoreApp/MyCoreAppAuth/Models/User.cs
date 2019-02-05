using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCoreAppAuth.Models
{
    public class User
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User AuthenticateUser(User user)
        {
            User userVal = null;
            if (user.Name == "Intesar" && user.Password == "1234")
            {
                userVal = new User { FullName = "Intesar Alam", Name = user.Name, Email = "alam.kir@gmail.com" };
            }
            return userVal;
        }
    }
}
