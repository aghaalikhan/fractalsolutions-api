using FractalSolutions.Api.Entities;

namespace FractalSolutions.Api.Repositories
{
    public interface IUserRepository
    {
        void AddUser(UserEntity user);
        void Remove(string userId);
        bool UserExists(string userId);
    }
}