using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.ViewComponents.MysteriousEvents
{
    public class MysteriousEventsInHomePage : ViewComponent
    {
        private readonly IMysteriousEvent _mysteriousEvent;

        public MysteriousEventsInHomePage(IMysteriousEvent mysteriousEvent)
        {
            _mysteriousEvent = mysteriousEvent;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _mysteriousEvent.GetVisibleLast6EventsAsync();
            return View(events);
        }
    }
}
