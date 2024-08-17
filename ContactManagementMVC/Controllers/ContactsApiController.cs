//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ContactManagementMVC.Data;
//using ContactManagementMVC.Models;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ContactManagementMVC.Controllers.Api
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ContactsApiController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<IdentityUser> _userManager;

//        public ContactsApiController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return Unauthorized();
//            }

//            return await _context.Contacts.Where(c => c.UserId == user.Id).ToListAsync();
//        }

//        // GET: api/Contacts/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Contact>> GetContact(int id)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return Unauthorized();
//            }

//            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);
//            if (contact == null)
//            {
//                return NotFound();
//            }

//            return contact;
//        }

//        // POST: api/Contacts
//        [HttpPost]
//        public async Task<ActionResult<Contact>> PostContact(Contact contact)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return Unauthorized();
//            }

//            contact.UserId = user.Id;
//            _context.Contacts.Add(contact);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutContact(int id, Contact contact)
//        {
//            if (id != contact.Id)
//            {
//                return BadRequest();
//            }

//            var user = await _userManager.GetUserAsync(User);
//            if (user == null || contact.UserId != user.Id)
//            {
//                return Unauthorized();
//            }

//            _context.Entry(contact).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ContactExists(id, user.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteContact(int id)
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return Unauthorized();
//            }

//            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);
//            if (contact == null)
//            {
//                return NotFound();
//            }

//            _context.Contacts.Remove(contact);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool ContactExists(int id, string userId)
//        {
//            return _context.Contacts.Any(e => e.Id == id && e.UserId == userId);
//        }
//    }
//}
