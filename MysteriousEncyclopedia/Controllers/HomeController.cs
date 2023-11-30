using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models;
using MysteriousEncyclopedia.Models.DTOs.ContactDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using System.Diagnostics;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMysteriousEvent _mysteriousEvent;
        private readonly IResource _resource;
        private readonly IContact _contact;

        public HomeController(ILogger<HomeController> logger, IMysteriousEvent mysteriousEvent, IResource resource, IContact contact)
        {
            _logger = logger;
            _mysteriousEvent = mysteriousEvent;
            _resource = resource;
            _contact = contact;
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

        public async Task<IActionResult> HomeMysteriousEventDetail(int id)
        {
            var mystery = await _mysteriousEvent.GetVisibleItemAsync(id);
            return View(mystery);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
