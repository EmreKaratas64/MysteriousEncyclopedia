using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.AccountDto;

namespace MysteriousEncyclopedia.Controllers
{
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(Context context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (ModelState.IsValid)
            {
                var IsExist = await _userManager.FindByNameAsync(signUpDto.username);
                if (IsExist != null)
                    ModelState.AddModelError("", "Username is already taken");
                IdentityUser user = new IdentityUser()
                {
                    UserName = signUpDto.username,
                    Email = signUpDto.email,
                };
                var result = await _userManager.CreateAsync(user, signUpDto.password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("SignIn");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(signUpDto);
        }

        public IActionResult SignIn()
        {
            return View();
        }

    }
}
