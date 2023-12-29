using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.ViewComponents.Comment
{
    public class EventDetailCommentList : ViewComponent
    {
        private readonly IComment _comment;

        public EventDetailCommentList(IComment comment)
        {
            _comment = comment;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var comments = await _comment.GetVisibleCommentsAsync(id);
            ViewBag.NumberofComments = await _comment.NumberOfCommentsByEventAsync(id);
            return View(comments);
        }
    }
}
