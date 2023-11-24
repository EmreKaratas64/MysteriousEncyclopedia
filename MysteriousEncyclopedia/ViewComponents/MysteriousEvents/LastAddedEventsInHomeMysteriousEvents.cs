using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.ViewComponents.MysteriousEvents
{
    public class LastAddedEventsInHomeMysteriousEvents : ViewComponent
    {
        private readonly IMysteriousEvent _mysteriousEvent;

        public LastAddedEventsInHomeMysteriousEvents(IMysteriousEvent mysteriousEvent)
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
