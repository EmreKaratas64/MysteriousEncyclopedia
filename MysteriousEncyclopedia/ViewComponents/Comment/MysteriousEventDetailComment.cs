using Microsoft.AspNetCore.Mvc;

namespace MysteriousEncyclopedia.ViewComponents.Comment
{
    public class MysteriousEventDetailComment : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
