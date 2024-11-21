using Microsoft.EntityFrameworkCore;
using Registration.Data;
using Registration.DTO.User;
using Registration.Helpers;
using Registration.Interface;
using Registration.Models;


namespace Registration.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<int> GetCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public IQueryable<User> GetAllUsers()
        {
            return _context.Users.Include(c => c.Contacts).AsQueryable();

        }
        public async Task<IEnumerable<User>> GetUsers(PaginationParams pageParams)
        {
            var users = await GetAllUsers().ToListAsync();

            return users
                .Skip((pageParams.Page - 1) * pageParams.PageSize)
                .Take(pageParams.PageSize);
        }

        public async Task<IEnumerable<User>> SearchUsers(QueryParams queryParams)
        {
            var users = GetAllUsers();

            if (!string.IsNullOrWhiteSpace(queryParams.Name))
            {
                users = users.Where(c => c.FirstName.Contains(queryParams.Name));
            }
            if (!string.IsNullOrWhiteSpace(queryParams.PhoneNumber))
            {
                users = users.Where(c => c.PhoneNumber.Contains(queryParams.PhoneNumber));
            }
            return await users.ToListAsync();

        }

        public async Task<UserWithContactDTO> GetUserWithContacts(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            var contacts = await _context.Contacts.Where(c => c.UserId == id).ToListAsync();

            return new UserWithContactDTO
            {
                Id = user.Id,
                Name = user.FirstName,
                Contacts = contacts,

            };
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            Save();
        }

        public void Delete(User user)
        {
            _context.Remove(user);
            Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }


    }
}
