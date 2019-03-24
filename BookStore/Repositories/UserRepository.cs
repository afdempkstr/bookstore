using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using BookStore.Domain.Models;
using Dapper;

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
            var users = new Dictionary<int, User>();

            Connection.Query<User, Role, User>(
                @"SELECT u.*, ur.RoleId, r.Id, r.Name FROM [User] u
                LEFT JOIN [UserRoles] ur on ur.UserId = u.Id
                LEFT JOIN [Role] r ON r.Id = ur.RoleId",
                (user, role) =>
                {
                    if (users.TryGetValue(user.Id, out var existingUser))
                    {
                        existingUser.Roles.Add(role);
                        return existingUser;
                    }
                    else
                    {
                        user.Roles.Add(role);
                        users.Add(user.Id, user);
                        return user;
                    }
                },
                splitOn: "RoleId");

            return users.Values.ToList();
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