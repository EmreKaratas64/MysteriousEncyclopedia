using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models.DTOs.AccountDto;

namespace MysteriousEncyclopedia.Controllers
{
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
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

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(signInDto.username);
                if (user == null) return View(signInDto);
                bool isUser = await _userManager.IsInRoleAsync(user, "User");
                if (isUser)
                {
                    var result = await _signInManager.PasswordSignInAsync(signInDto.username, signInDto.password, false, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("HomePage", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt!");
                        return View(signInDto);
                    }
                }
                ModelState.AddModelError("", "Your account might have been banned!");
            }
            return View(signInDto);
        }

    }
}
