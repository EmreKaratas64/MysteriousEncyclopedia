using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DTOs.ReferenceDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReferenceController : Controller
    {
        private readonly IResource _resource;

        public ReferenceController(IResource resource)
        {
            _resource = resource;
        }

        public async Task<IActionResult> ReferencesList(int page = 1)
        {
            var values = await _resource.GetAllAsync();
            return View(values.ToPagedList(page, 10));
        }

        [HttpGet]
        public IActionResult AddReference()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddReference(ReferencesDto reference)
        {
            if (ModelState.IsValid)
            {
                _resource.CreateAsync(reference);
                return RedirectToAction("ReferencesList");
            }
            return View(reference);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateReference(int id)
        {
            var value = await _resource.GetItemAsync(id);
            if (value != null)
                return View(value);
            return View();
        }

        [HttpPost]
        public IActionResult UpdateReference(ReferencesDto reference)
        {
            if (ModelState.IsValid)
            {
                _resource.UpdateAsync(reference);
                return RedirectToAction("ReferencesList");
            }
            return View(reference);
        }
    }
}
