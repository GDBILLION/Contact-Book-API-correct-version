using Registration.DTO.User;
using Registration.Helpers;
using Registration.Models;

namespace Registration.Interface
{
    public interface IUserRepository
    {
        void Delete(User user);
        IQueryable<User> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<UserWithContactDTO> GetUserWithContacts(string id);
        Task<IEnumerable<User>> GetUsers(PaginationParams pageParams);
        Task<int> GetCountAsync();
        bool Save();
        Task<IEnumerable<User>> SearchUsers(QueryParams queryParams);
        void UpdateUser(User user);
    }
}
