using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using BookStore.Domain.Models;

namespace BookStore.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(DbConnection connection, IBookStoreDb parent)
            : base(connection, parent)
        {
        }

        public override IEnumerable<User> All()
        {
            return AllWith<Role>((user, role) => user.Roles.Add(role));
        }

        public override User Create(User item)
        {
            throw new NotImplementedException();
        }

        public override bool Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}