using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models;
using MysteriousEncyclopedia.Models.DTOs.AccountDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUser _user;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserStore<IdentityUser> userStore, IUser user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _user = user;
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

        [HttpGet]
        public IActionResult Setting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Setting(UserSettingViewModel userSetting)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _userManager.ChangePasswordAsync(user, userSetting.currentpassword, userSetting.password);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignOut");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(userSetting);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }


        public async Task<IActionResult> UserList(int page = 1)
        {
            var users = await _user.GetAllAsync();
            return View(users.ToPagedList(page, 10));
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest();
            var roles = await _user.GetAllRolesAsync();

            TempData["UserId"] = user.Id;
            ViewBag.UserName = user.UserName;
            var user_roles = await _userManager.GetRolesAsync(user);

            List<AssignRoleViewModel> model = new List<AssignRoleViewModel>();

            foreach (var item in roles)
            {
                AssignRoleViewModel m = new AssignRoleViewModel();
                m.RoleID = item.Id;
                m.RoleName = item.Name;
                m.Exists = user_roles.Contains(item.Name);
                model.Add(m);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserRoles(List<AssignRoleViewModel> assignRole)
        {
            var userId = TempData["UserId"];
            var user = await _userManager.FindByIdAsync(userId.ToString());

            foreach (var item in assignRole)
            {
                if (item.Exists)
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                else
                    _user.RemoveUserFromRoleAsync(user.Id, item.RoleID);
            }
            return RedirectToAction("UserList");
        }



    }
}
