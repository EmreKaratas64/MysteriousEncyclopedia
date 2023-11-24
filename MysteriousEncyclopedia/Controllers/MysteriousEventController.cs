using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DTOs.EventDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    public class MysteriousEventController : Controller
    {
        private readonly IMysteriousEvent _mysteriousEvent;

        public MysteriousEventController(IMysteriousEvent mysteriousEvent)
        {
            _mysteriousEvent = mysteriousEvent;
        }

        public async Task<IActionResult> MysteriousEventList(int page = 1)
        {
            var mysteriousEvents = await _mysteriousEvent.GetAllAsync();
            return View(mysteriousEvents.ToPagedList(page, 9));
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
            return View();
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
