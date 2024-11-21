using Registration.Helpers;
using Registration.Models;

namespace Registration.Interface
{
    public interface IContactRepository
    {
        public Task<int> GetCountAsync();
        public IQueryable<Contact> GetAllContacts(string userId);
        public Task<IEnumerable<Contact>> GetContacts(PaginationParams pageParams, string userId);
        public Task<IEnumerable<Contact>> SearchContacts(QueryParams queryParams, string userId);
        Task<Contact> GetContactById(int id);
        void Add(Contact contact);
        void Delete(Contact contact);
        void UpdateContact(Contact contact);
        bool Save();
    }
}
