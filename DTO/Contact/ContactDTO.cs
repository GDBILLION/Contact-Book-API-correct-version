using System.ComponentModel.DataAnnotations;

namespace Registration.DTO.Contact;

    public class ContactDTO
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    public string? Address { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}


