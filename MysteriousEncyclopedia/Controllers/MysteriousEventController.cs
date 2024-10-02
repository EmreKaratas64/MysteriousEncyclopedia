using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DTOs.EventDto;
using MysteriousEncyclopedia.Models.DTOs.ReferenceDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MysteriousEventController : Controller
    {
        private readonly IMysteriousEvent _mysteriousEvent;
        private readonly IResource _resource;

        public MysteriousEventController(IMysteriousEvent mysteriousEvent, IResource resource)
        {
            _mysteriousEvent = mysteriousEvent;
            _resource = resource;
        }

        public async Task<IActionResult> MysteriousEventList(int page = 1)
        {
            var mysteriousEvents = await _mysteriousEvent.GetAllAsync();
            return View(mysteriousEvents.ToPagedList(page, 12));
        }

        public async Task<IActionResult> ResourcesByEventID(int id, int page = 1)
        {
            var resources = await _resource.GetResourcesWithEventAndReferenceByEventIdAsync(id);
            ViewBag.eventId = id;
            return View(resources.ToPagedList(page, 10));
        }

        [HttpGet]
        public IActionResult AddResourceToEvent(int id)
        {
            MysteriousEventReferenceDto model = new MysteriousEventReferenceDto
            {
                EventID = id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddResourceToEvent(MysteriousEventReferenceDto mysteriousEventReference)
        {
            var reference = await _resource.GetItemAsync(mysteriousEventReference.ReferenceID);
            if (reference == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                _resource.AddReferenceToTheEventAsync(mysteriousEventReference);
                return RedirectToAction("ResourcesByEventID", new { id = mysteriousEventReference.EventID });
            }
            return View(mysteriousEventReference);
        }

        public async Task<IActionResult> DeleteResourceOfEvent(int eventId, int referenceId)
        {
            var reference = await _resource.GetItemAsync(referenceId);
            var eventt = await _mysteriousEvent.GetItemAsync(eventId);
            if (reference != null || eventt != null)
                _resource.DeleteMysteriousEventReferenceAsync(eventId, referenceId);
            return RedirectToAction("ResourcesByEventID", new { id = eventId });

        }

        [HttpGet]
        public IActionResult MysteriousEventAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MysteriousEventAdd(MysteriousEventDto mysteriousEvent)
        {
            if (ModelState.IsValid)
            {
                mysteriousEvent.EventModifiedDate = DateTime.Now;
                _mysteriousEvent.CreateAsync(mysteriousEvent);
                return RedirectToAction("MysteriousEventList");
            }
            return View(mysteriousEvent);
        }

        [HttpGet]
        public async Task<IActionResult> MysteriousEventUpdate(int id)
        {
            var mystery = await _mysteriousEvent.GetItemAsync(id);
            if (mystery != null)
                return View(mystery);
            return RedirectToAction("MysteriousEventList");
        }

        [HttpPost]
        public async Task<IActionResult> MysteriousEventUpdate(MysteriousEventDto mysteriousEvent)
        {
            if (ModelState.IsValid)
            {
                mysteriousEvent.EventModifiedDate = DateTime.Now;
                _mysteriousEvent.UpdateAsync(mysteriousEvent);
                return RedirectToAction("MysteriousEventList");
            }
            return View(mysteriousEvent);
        }
    }
}
