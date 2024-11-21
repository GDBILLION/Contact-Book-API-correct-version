namespace Registration.DTO.User
{
    public class UserWithContactDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Models.Contact> Contacts { get; set; }

    }
}
