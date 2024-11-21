using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Registration.DTO.Contact;
using Registration.Helpers;
using Registration.Interface;
using Registration.Models;
using System.Security.Claims;

namespace Registration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        private readonly IPhotoService _photoService;
        public ContactController(IContactRepository contactRepository, IPhotoService photoService)
        {
            _contactRepository = contactRepository;
            _photoService = photoService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            int totalContacts = await _contactRepository.GetCountAsync();

            int totalPages = (int)Math.Ceiling((double)totalContacts / paginationParams.PageSize);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var contacts = await _contactRepository.GetContacts(paginationParams, userId);

            if (contacts == null)
            {
                return Ok("Contact list empty");
            }

            var response = new
            {
                TotalContacts = totalContacts,
                CurrentPage = paginationParams.Page,
                TotalPages = totalPages,
                Contacts = contacts,
            };

            return Ok(response);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSearchedContact([FromQuery] QueryParams queryParams)
        {
            if (queryParams == null)
            {
                return BadRequest("Please input a search term");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var contacts = await _contactRepository.SearchContacts(queryParams, userId);

            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            if (id == 0) return BadRequest("id cannot be zero");

            var contact = await _contactRepository.GetContactById(id);

            if (contact == null) return BadRequest("Cannot find contact");

            return Ok(contact);
        }

        [HttpPost("add")]
        public IActionResult CreateContact([FromBody] ContactDTO contactDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Address = contactDto.Address,
                PhoneNumber = contactDto.PhoneNumber,
                UserId = userId,
            };

            _contactRepository.Add(contact);
            return Ok(contact);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> EditContact(int id, [FromBody] ContactDTO contactDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _contactRepository.GetContactById(id);

            if (contact == null)
            {
                return NotFound();
            }

            contact.FirstName = contactDTO.FirstName;
            contact.LastName = contactDTO.LastName;
            contact.PhoneNumber = contactDTO.PhoneNumber;
            contact.Address = contactDTO.Address;

            _contactRepository.UpdateContact(contact);

            return Ok("Updated successfully");
        }

        [HttpPatch("photo/{id}")]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile photo)
        {
            if (photo == null || photo.Length == 0) return BadRequest("No photo file uploaded.");

            var contact = await _contactRepository.GetContactById(id);

            if (contact == null)
            {
                return NotFound($"No contact found with ID = {id}");
            }

            var uploadResult = await _photoService.AddPhotoAsync(photo);

            if (uploadResult == null)
            {
                return StatusCode(500, "Photo upload failed");
            }
            contact.ContactPhoto = uploadResult.Url.AbsoluteUri;

            _contactRepository.UpdateContact(contact);

            return Ok(new
            {
                Message = "Photo uploaded Successfully.",
                PhotoUrl = contact.ContactPhoto
            });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _contactRepository.GetContactById(id);

            if (contact == null) return BadRequest("Cannot find contact to delete");

            _contactRepository.Delete(contact);

            return Ok("Contact deleted successfully");
        }

    }
}
