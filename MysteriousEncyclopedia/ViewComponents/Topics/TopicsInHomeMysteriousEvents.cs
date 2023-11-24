using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.ViewComponents.Topics
{
    public class TopicsInHomeMysteriousEvents : ViewComponent
    {
        private readonly ITopic _topic;

        public TopicsInHomeMysteriousEvents(ITopic topic)
        {
            _topic = topic;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _topic.GetTopicsWithNumberOfEventsAsync();
            return View(values);
        }
    }
}
