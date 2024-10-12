using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models;
using MysteriousEncyclopedia.Models.DTOs.Comment;
using MysteriousEncyclopedia.Models.DTOs.ContactDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using System.Diagnostics;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMysteriousEvent _mysteriousEvent;
        private readonly IResource _resource;
        private readonly IContact _contact;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IComment _comment;

        public HomeController(ILogger<HomeController> logger, IMysteriousEvent mysteriousEvent, IResource resource, IContact contact, UserManager<IdentityUser> userManager, IComment comment)
        {
            _logger = logger;
            _mysteriousEvent = mysteriousEvent;
            _resource = resource;
            _contact = contact;
            _userManager = userManager;
            _comment = comment;
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public async Task<IActionResult> HomeMysteriousEventList(int page = 1)
        {
            var events = await _mysteriousEvent.GetVisibleEventsAsync();
            return View(events.ToPagedList(page, 10));
        }

        public async Task<IActionResult> HomeMysteriousEventsByTopic(string topic, int page = 1)
        {
            var events = await _mysteriousEvent.GetVisibleEventsByTopicAsync(topic);
            ViewBag.topic = topic;
            return View(events.ToPagedList(page, 10));
        }

        public async Task<IActionResult> HomeMysteriousEventsSearch(string eventName)
        {
            var searchedEvents = await _mysteriousEvent.GetVisibleEventsByName(eventName);
            return View(searchedEvents);
        }

        public async Task<IActionResult> HomeMysteriousEventDetail(int id)
        {
            var mystery = await _mysteriousEvent.GetVisibleItemAsync(id);
            return View(mystery);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentDto commentDto)
        {
            if (User.Identity.Name == "" || User.Identity.Name == null) return RedirectToAction("SignIn", "Account");
            else
            {
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                commentDto.UserId = currentUser.Id;
                if (ModelState.IsValid)
                {
                    _comment.CreateAsync(commentDto);
                    return RedirectToAction("HomePage", "Home");
                }
                return RedirectToAction("HomeMysteriousEventDetail", new { id = commentDto.MysteryID });
            }
        }


        public async Task<IActionResult> HomeResources(int page = 1)
        {
            var resources = await _resource.GetAllWithEventAndReferenceAsync();
            return View(resources.ToPagedList(page, 12));
        }

        public IActionResult HomeContact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HomeContact(ContactsDto contact)
        {
            if (ModelState.IsValid)
            {
                contact.ContactDate = DateTime.Now;
                _contact.CreateAsync(contact);
                TempData["ContactSuccess"] = "Your message has been sent";
                return View();
            }
            TempData["ContactFail"] = "Sorry, your message could not be sent :(";
            return View(contact);
        }

        public IActionResult ErrorPage(string code)
        {
            if (code == "404")
            {
                ViewBag.codee = "404 - Not Found";
                ViewBag.codeMess = "Sorry, we couldn't find the page you are looking!";
            }

            else if (code == "304")
            {
                ViewBag.codee = "304 - Not Modified";
                ViewBag.codeMess = "The content is not modified!";
            }
            else if (code == "400")
            {
                ViewBag.codee = "400 - Bad Request";
                ViewBag.codeMess = "The request is not valid!";
            }
            else if (code == "401" || code == "403")
                ViewBag.codeMess = "The Content is forbidden or you're not allowed to view!";
            else
                ViewBag.codeMess = "Sorry, Something went wrong!";
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
