using Microsoft.EntityFrameworkCore;
using Registration.Data;
using Registration.Helpers;
using Registration.Interface;
using Registration.Models;

namespace Registration.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;
        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<int> GetCountAsync()
        {
            return await _context.Contacts.CountAsync();
        }

        public IQueryable<Contact> GetAllContacts(string userId)
        {
            return _context.Contacts.AsQueryable();

        }

        public async Task<IEnumerable<Contact>> GetContacts(PaginationParams pageParams, string userId)
        {
            var contacts = GetAllContacts(userId);

            return await contacts
                .Skip((pageParams.Page - 1) * pageParams.PageSize)
                .Take(pageParams.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contact>> SearchContacts(QueryParams queryParams, string userId)
        {
            var contacts = GetAllContacts(userId);

            if (!string.IsNullOrWhiteSpace(queryParams.Name))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(queryParams.Name));
            }
            if (!string.IsNullOrWhiteSpace(queryParams.PhoneNumber))
            {
                contacts = contacts.Where(c => c.PhoneNumber.Contains(queryParams.PhoneNumber));
            }
            return await contacts.ToListAsync();

        }

        public async Task<Contact> GetContactById(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public void Add(Contact contact)
        {
            _context.Add(contact);
            Save();
        }

        public void UpdateContact(Contact contact)
        {
            _context.Update(contact);
            Save();
        }

        public void Delete(Contact contact)
        {
            _context.Remove(contact);
            Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

       

       
    }
}
