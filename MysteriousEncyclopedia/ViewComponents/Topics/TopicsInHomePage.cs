using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.ViewComponents.Topics
{
    public class TopicsInHomePage : ViewComponent
    {
        private readonly ITopic _topic;

        public TopicsInHomePage(ITopic topic)
        {
            _topic = topic;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topics = await _topic.GetAllAsync(); ;
            return View(topics);
        }
    }
}
