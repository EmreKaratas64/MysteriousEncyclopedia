using Microsoft.AspNetCore.Mvc;

namespace MysteriousEncyclopedia.ViewComponents.Tags
{
    public class HomeTagsOnSidebar : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
