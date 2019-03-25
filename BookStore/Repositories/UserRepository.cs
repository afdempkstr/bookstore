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
            // note: creating a user does not set their credentials, the
            // SetUserCredentials stored proc must be called after the user is created
            var query = $@"INSERT INTO {TableName} (Username, Name, RegisteredAt)
                        VALUES(@Username, @Name, SYSDATETIMEOFFSET()); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var insertedItemId = Connection.ExecuteScalar<int>(query, new
            {
                item.Username,
                item.Name
            });
            item.Id = insertedItemId;

            //associate user with roles
            query = @"INSERT INTO UserRoles(UserId, RoleId) VALUES (@UserId, @RoleId)";

            foreach (var role in item.Roles)
            {
                Connection.ExecuteAsync(query, new
                {
                    UserId = item.Id,
                    RoleId = role.Id
                });
            }

            return item;
        }

        public override bool Update(User item)
        {
            // we can only update an existing user. Any existing user has an Id > 0
            if (item.Id <= 0)
            {
                return false;
            }

            // note: updating a user does not (re)set their credentials, the
            // SetUserCredentials stored proc must be called after the user is updated
            var query = $@"UPDATE {TableName} SET Name = @Name WHERE {TableName}.Id = @Id";
            var affectedRows = Connection.Execute(query, new
            {
                item.Name,
                item.Id
            });

            if (affectedRows <= 0)
            {
                return false;
            }

            //clear existing user roles
            query = "DELETE FROM UserRoles WHERE UserId = @Id";
            affectedRows = Connection.Execute(query, new {item.Id});

            //associate user with roles
            query = @"INSERT INTO UserRoles(UserId, RoleId) VALUES (@UserId, @RoleId)";

            foreach (var role in item.Roles)
            {
                Connection.ExecuteAsync(query, new
                {
                    UserId = item.Id,
                    RoleId = role.Id
                });
            }

            return true;
        }
    }
}