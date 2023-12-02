using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContact _contact;

        public ContactController(IContact contact)
        {
            _contact = contact;
        }

        public async Task<IActionResult> ListContacts(int page = 1)
        {
            var contacts = await _contact.GetAllAsync();
            return View(contacts.ToPagedList(page, 10));
        }

        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _contact.GetItemAsync(id);
            if (contact != null)
                _contact.DeleteContactAsync(id);

            return RedirectToAction("ListContacts");
        }
    }
}
