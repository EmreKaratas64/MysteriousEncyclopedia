using Microsoft.AspNetCore.Authorization;
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
        private readonly IComment _comment;

        public ContactController(IContact contact, IRequest request, UserManager<IdentityUser> userManager, IComment comment)
        {
            _contact = contact;
            _request = request;
            _userManager = userManager;
            _comment = comment;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ListContacts(int page = 1)
        {
            var contacts = await _contact.GetAllAsync();
            return View(contacts.ToPagedList(page, 10));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _contact.GetItemAsync(id);
            if (contact != null)
                _contact.DeleteContactAsync(id);

            return RedirectToAction("ListContacts");
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> MakeRequest(int Id)
        {
            if (User.Identity.Name == null || User.Identity.Name == "") return RedirectToAction("SignIn", "Account");
            RequestDto requestDto = new RequestDto();
            requestDto.RequestEventId = Id;
            return View(requestDto);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> MakeRequest(RequestDto requestDto)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RequestList(int page = 1)
        {
            var requests = await _request.GetAllAsync();
            return View(requests.ToPagedList(page, 10));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RequestDetail(int id)
        {
            var requestt = await _request.GetItemAsync(id);
            if (requestt == null) return BadRequest();
            return View(requestt);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteRequest(int Id)
        {
            var requestt = await _request.GetItemAsync(Id);
            if (requestt == null) return BadRequest();
            _request.DeleteRequestAsync(Id);
            return RedirectToAction("RequestList");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var requestt = await _request.GetItemAsync(id);
            if (requestt == null) return BadRequest();
            requestt.RequestStatus = "approved";
            _request.UpdateAsync(requestt);
            return RedirectToAction("RequestList");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var requestt = await _request.GetItemAsync(id);
            if (requestt == null) return BadRequest();
            requestt.RequestStatus = "canceled";
            _request.UpdateAsync(requestt);
            return RedirectToAction("RequestList");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ListComments(int page = 1)
        {
            var comments = await _comment.GetAllAsync();
            return View(comments.ToPagedList(page, 10));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CommentDetail(int id)
        {
            var comment = await _comment.GetItemAsync(id);
            if (comment == null) return BadRequest();
            return View(comment);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _comment.GetItemAsync(id);
            if (comment == null) return BadRequest();
            _comment.DeleteCommentAsync(id);
            return RedirectToAction("ListComments");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AcceptComment(int id)
        {
            var comment = await _comment.GetItemAsync(id);
            if (comment == null) return BadRequest();
            comment.CommentStatus = true;
            _comment.UpdateAsync(comment);
            return RedirectToAction("ListComments");
        }
    }
}
