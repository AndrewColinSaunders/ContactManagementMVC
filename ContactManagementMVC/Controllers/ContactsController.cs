using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactManagementMVC.Data;
using ContactManagementMVC.Models;
using Ganss.Xss;

namespace ContactManagementMVC.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ContactsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public IActionResult Add()
        {
            return PartialView("_AddContactForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Contact contact)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            contact.UserId = user.Id;

            var sanitizer = new HtmlSanitizer();
            contact.Name = sanitizer.Sanitize(contact.Name);
            contact.Email = sanitizer.Sanitize(contact.Email);
            contact.PhoneNumber = sanitizer.Sanitize(contact.PhoneNumber);
            contact.Address = sanitizer.Sanitize(contact.Address);

            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("_AddContactForm", contact);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null || contact.UserId != user.Id)
            {
                return NotFound();
            }

            return PartialView("_EditContactForm", contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Contact contact)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            contact.UserId = user.Id;

            var sanitizer = new HtmlSanitizer();
            contact.Name = sanitizer.Sanitize(contact.Name);
            contact.Email = sanitizer.Sanitize(contact.Email);
            contact.PhoneNumber = sanitizer.Sanitize(contact.PhoneNumber);
            contact.Address = sanitizer.Sanitize(contact.Address);

            if (ModelState.IsValid)
            {
                _context.Contacts.Update(contact);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("_EditContactForm", contact);
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var contacts = _context.Contacts.Where(c => c.UserId == user.Id).ToList();
            return View(contacts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null || contact.UserId != user.Id)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sort()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var contacts = _context.Contacts.Where(c => c.UserId == user.Id).ToList();

            // Bubble Sort 
            for (int i = 0; i < contacts.Count - 1; i++)
            {
                for (int j = 0; j < contacts.Count - i - 1; j++)
                {
                    if (string.Compare(contacts[j].Name, contacts[j + 1].Name) > 0)
                    {
                        var temp = contacts[j];
                        contacts[j] = contacts[j + 1];
                        contacts[j + 1] = temp;
                    }
                }
            }

            return View("Index", contacts);
        }
    }
}
