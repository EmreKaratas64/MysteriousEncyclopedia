using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DTOs.Request;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContact _contact;
        private readonly IRequest _request;

        public ContactController(IContact contact, IRequest request)
        {
            _contact = contact;
            _request = request;
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

        [HttpGet]
        public async Task<IActionResult> MakeRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeRequest(RequestDto requestDto)
        {
            requestDto.RequestDate = DateTime.Now;
            requestDto.RequestStatus = "pending";
            if (ModelState.IsValid)
            {
                _request.CreateAsync(requestDto);
                TempData["RequestSuccess"] = "Your request has been sent, thank you";
                return RedirectToAction("HomePage", "Home");
            }
            TempData["RequestFail"] = "Your request could not be sent :(";
            return View(requestDto);


        }
    }
}
