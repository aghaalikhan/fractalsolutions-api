using FractalSolutions.Api.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FractalSolutions.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly IList<UserEntity> _users;

        public UserRepository()
        {
            _users = new List<UserEntity>();
        }

        public void AddUser(UserEntity user)
        {
            _users.Add(user);
        }

        public bool UserExists(string userId)
        {
            return _users.Any(x => x.UserId == userId);
        }

        public void Remove(string userId)
        {
            var userEntity = _users.FirstOrDefault(x => x.UserId == userId);
            _users.Remove(userEntity);
        }
    }
}