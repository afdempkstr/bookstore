using System;
using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public class User : Entity
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public DateTimeOffset RegisteredAt { get; set; }

        public IList<Role> Roles { get; }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}
