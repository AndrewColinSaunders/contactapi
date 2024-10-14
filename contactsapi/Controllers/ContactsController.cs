//using System.Security.Claims;
//using contactsapi.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Http;

//namespace contactsapi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class ContactsController : ControllerBase
//    {
//        private readonly ContactsContext context;
//        private readonly ILogger<ContactsController> logger;
//        private readonly string userId;

//        public ContactsController(ContactsContext context, ILogger<ContactsController> logger, IHttpContextAccessor httpContextAccessor)
//        {
//            this.context = context;
//            this.logger = logger;
//            this.userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
//        {
//            logger.LogInformation("User ID extracted from token: {UserId}", userId);

//            if (string.IsNullOrEmpty(userId))
//            {
//                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
//                return Unauthorized("User ID not found in token");
//            }

//            try
//            {
//                var userContacts = await context.Contacts
//                                                 .Where(c => c.UserId == int.Parse(userId))
//                                                 .ToListAsync();
//                logger.LogInformation("Fetched {ContactCount} contacts for user ID {UserId}.", userContacts.Count, userId);
//                return Ok(userContacts);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Error fetching contacts for user ID {UserId}", userId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Contact>> GetContactById(int id)
//        {
//            logger.LogInformation("User ID extracted from token for GetContactById: {UserId}", userId);

//            if (string.IsNullOrEmpty(userId))
//            {
//                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
//                return Unauthorized("User ID not found in token");
//            }

//            try
//            {
//                var contact = await this.context.Contacts
//                                             .Where(c => c.UserId == int.Parse(userId) && c.Id == id)
//                                             .FirstOrDefaultAsync();
//                if (contact == null)
//                {
//                    logger.LogWarning("Contact ID {ContactId} not found for user ID {UserId}.", id, userId);
//                    return NotFound();
//                }

//                logger.LogInformation("Contact ID {ContactId} retrieved successfully for user ID {UserId}.", id, userId);
//                return Ok(contact);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Error retrieving contact ID {ContactId} for user ID {UserId}.", id, userId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult<Contact>> CreateContact([FromBody] Contact contact)
//        {
//            if (string.IsNullOrEmpty(userId))
//            {
//                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
//                return Unauthorized("User ID not found in token");
//            }

//            logger.LogInformation($"Received contact details: FirstName={contact.FirstName}, LastName={contact.LastName ?? "null"}, Email={contact.Email}, Phone={contact.Phone ?? "null"}");

//            contact.UserId = int.Parse(userId);

//            this.context.Contacts.Add(contact);

//            try
//            {
//                await this.context.SaveChangesAsync();
//                logger.LogInformation($"Contact created successfully for user ID {userId}. Contact ID: {contact.Id}");

//                return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, $"Error creating contact for user ID {userId}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateContact(int id, Contact updatedContact)
//        {
//            logger.LogInformation("User ID extracted from token for UpdateContact: {UserId}", userId);

//            if (string.IsNullOrEmpty(userId))
//            {
//                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
//                return Unauthorized("User ID not found in token");
//            }

//            var contact = await this.context.Contacts
//                                        .Where(c => c.UserId == int.Parse(userId) && c.Id == id)
//                                        .FirstOrDefaultAsync();

//            if (contact == null)
//            {
//                logger.LogWarning("Contact ID {ContactId} not found for user ID {UserId}.", id, userId);
//                return NotFound();
//            }

//            contact.FirstName = updatedContact.FirstName;
//            contact.LastName = updatedContact.LastName;
//            contact.Email = updatedContact.Email;
//            contact.Phone = updatedContact.Phone;

//            try
//            {
//                await this.context.SaveChangesAsync();
//                logger.LogInformation("Contact ID {ContactId} updated successfully for user ID {UserId}.", id, userId);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Error updating contact ID {ContactId} for user ID {UserId}.", id, userId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteContact(int id)
//        {
//            logger.LogInformation("User ID extracted from token for DeleteContact: {UserId}", userId);

//            if (string.IsNullOrEmpty(userId))
//            {
//                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
//                return Unauthorized("User ID not found in token");
//            }

//            var contact = await context.Contacts
//                                        .Where(c => c.UserId == int.Parse(userId) && c.Id == id)
//                                        .FirstOrDefaultAsync();

//            if (contact == null)
//            {
//                logger.LogWarning("Contact ID {ContactId} not found for user ID {UserId}.", id, userId);
//                return NotFound();
//            }

//            context.Contacts.Remove(contact);

//            try
//            {
//                await context.SaveChangesAsync();
//                logger.LogInformation("Contact ID {ContactId} deleted successfully for user ID {UserId}.", id, userId);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Error deleting contact ID {ContactId} for user ID {UserId}.", id, userId);
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}

using System.Security.Claims;
using contactsapi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace contactsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsContext context;
        private readonly ILogger<ContactsController> logger;
        private readonly string userId;

        public ContactsController(ContactsContext context, ILogger<ContactsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.logger = logger;
            this.userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            logger.LogInformation("User ID extracted from token: {UserId}", userId);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
                return Unauthorized("User ID not found in token");
            }

            try
            {
                var userContacts = await context.Contacts
                                                 .Where(c => c.UserId == int.Parse(userId))
                                                 .ToListAsync();
                logger.LogInformation("Fetched {ContactCount} contacts for user ID {UserId}.", userContacts.Count, userId);
                return Ok(userContacts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching contacts for user ID {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContactById(int id)
        {
            logger.LogInformation("User ID extracted from token for GetContactById: {UserId}", userId);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
                return Unauthorized("User ID not found in token");
            }

            try
            {
                var contact = await context.Contacts
                                             .Where(c => c.UserId == int.Parse(userId) && c.Id == id)
                                             .FirstOrDefaultAsync();
                if (contact == null)
                {
                    logger.LogWarning("Contact ID {ContactId} not found for user ID {UserId}.", id, userId);
                    return NotFound();
                }

                logger.LogInformation("Contact ID {ContactId} retrieved successfully for user ID {UserId}.", id, userId);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving contact ID {ContactId} for user ID {UserId}.", id, userId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact([FromBody] Contact contact)
        {
            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
                return Unauthorized("User ID not found in token");
            }

            logger.LogInformation($"Received contact details: FirstName={contact.FirstName}, LastName={contact.LastName ?? "null"}, Email={contact.Email}, Phone={contact.Phone ?? "null"}");

            contact.UserId = int.Parse(userId);

            context.Contacts.Add(contact);

            try
            {
                await context.SaveChangesAsync();
                logger.LogInformation($"Contact created successfully for user ID {userId}. Contact ID: {contact.Id}");

                return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating contact for user ID {userId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact updatedContact)
        {
            logger.LogInformation("User ID extracted from token for UpdateContact: {UserId}", userId);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
                return Unauthorized("User ID not found in token");
            }

            var contact = await context.Contacts
                                        .Where(c => c.UserId == int.Parse(userId) && c.Id == id)
                                        .FirstOrDefaultAsync();

            if (contact == null)
            {
                logger.LogWarning("Contact ID {ContactId} not found for user ID {UserId}.", id, userId);
                return NotFound();
            }

            contact.FirstName = updatedContact.FirstName;
            contact.LastName = updatedContact.LastName;
            contact.Email = updatedContact.Email;
            contact.Phone = updatedContact.Phone;
            contact.ProfileImage = updatedContact.ProfileImage;

            try
            {
                await context.SaveChangesAsync();
                logger.LogInformation("Contact ID {ContactId} updated successfully for user ID {UserId}.", id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating contact ID {ContactId} for user ID {UserId}.", id, userId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            logger.LogInformation("User ID extracted from token for DeleteContact: {UserId}", userId);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("Unauthorized access attempt - User ID not found in token.");
                return Unauthorized("User ID not found in token");
            }

            var contact = await context.Contacts
                                        .Where(c => c.UserId == int.Parse(userId) && c.Id == id)
                                        .FirstOrDefaultAsync();

            if (contact == null)
            {
                logger.LogWarning("Contact ID {ContactId} not found for user ID {UserId}.", id, userId);
                return NotFound();
            }

            context.Contacts.Remove(contact);

            try
            {
                await context.SaveChangesAsync();
                logger.LogInformation("Contact ID {ContactId} deleted successfully for user ID {UserId}.", id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting contact ID {ContactId} for user ID {UserId}.", id, userId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}