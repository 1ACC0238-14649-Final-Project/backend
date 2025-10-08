using System;
using System.Collections.Generic;
using gigu_back_end.Shared.Domain.Model.Entities;

namespace gigu_back_end.User.Domain.Models.Entities
{
    public class User : BaseEntity
    {
        public User(string name, string lastname, string email, string password, string role, string image)
        {
            Name = name;
            Lastname = lastname;
            Email = email;
            Password = password;
            Role = role;
            Image = image;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public User()
        {
            
        }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
    }
}