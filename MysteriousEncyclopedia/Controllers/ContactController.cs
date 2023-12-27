using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public ContactController(IContact contact, IRequest request, UserManager<IdentityUser> userManager)
        {
            _contact = contact;
            _request = request;
            _userManager = userManager;
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
        public async Task<IActionResult> MakeRequest(int Id)
        {
            RequestDto requestDto = new RequestDto();
            requestDto.RequestEventId = Id;
            return View(requestDto);
        }

        [HttpPost]
        public async Task<IActionResult> MakeRequest(RequestDto requestDto)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null) return BadRequest();

            requestDto.RequestUserId = currentUser.Id;

            requestDto.RequestStatus = "pending";
            if (ModelState.IsValid)
            {
                _request.CreateAsync(requestDto);
                TempData["RequestSuccess"] = "Your request has been sent, thank you";
                return View();
            }
            TempData["RequestFail"] = "Your request could not be sent :(";
            return View(requestDto);


        }
    }
}
