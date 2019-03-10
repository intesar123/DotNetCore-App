using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCoreAppAuth.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

        public User AuthenticateUser(User user)
        {
            User userVal = null;
            if (user.Email == "alam.kir@gmail.com" && user.Password == "Hello@123")
            {
                userVal = new User { FullName = "Intesar Alam", UserName = user.UserName, Email = "alam.kir@gmail.com" };
            }
            return userVal;
        }
    }
}
