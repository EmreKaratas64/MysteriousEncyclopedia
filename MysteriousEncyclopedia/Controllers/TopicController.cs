using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DTOs.TopicDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    //viewing controller Ctrl + m + g
    public class TopicController : Controller
    {
        private readonly ITopic _topic;

        public TopicController(ITopic topic)
        {
            _topic = topic;
        }

        public async Task<IActionResult> TopicList(int page = 1)
        {
            var topics = await _topic.GetAllAsync();
            return View(topics.ToPagedList(page, 10));
        }

        [HttpGet]
        public IActionResult TopicAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TopicAdd(ResultTopicDto topic)
        {
            if (ModelState.IsValid)
            {
                _topic.CreateAsync(topic);
                return RedirectToAction("TopicList");
            }
            return View(topic);
        }

        [HttpGet]
        public async Task<IActionResult> TopicUpdate(int id)
        {
            var result = await _topic.GetItemAsync(id);
            if (result != null)
                return View(result);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TopicUpdate(ResultTopicDto topic)
        {
            if (ModelState.IsValid)
            {
                _topic.UpdateAsync(topic);
                return RedirectToAction("TopicList");
            }
            return View(topic);
        }
    }
}
