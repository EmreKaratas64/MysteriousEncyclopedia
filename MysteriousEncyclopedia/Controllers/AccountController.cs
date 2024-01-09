using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MysteriousEncyclopedia.Models;
using MysteriousEncyclopedia.Models.DTOs.AccountDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;
using X.PagedList;

namespace MysteriousEncyclopedia.Controllers
{
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

        MailService ms = new MailService();
        Random random = new Random();

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
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
                    PhoneNumber = random.NextInt64(100000, 1000000).ToString()
                };
                var result = await _userManager.CreateAsync(user, signUpDto.password);
                if (result.Succeeded)
                {
                    ms.SendMail("Mysterious Encyclopedia - Account Register", "Hello\nThank you for registering the Mysterious Encyclopedia\n Here is your confirmation code: " + user.PhoneNumber, "emrekaratas076@gmail.com", user.Email);
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("MailConfirm");
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminLogin(SignInDto signInDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(signInDto.username);
                if (user == null) return View(signInDto);
                bool isUser = await _userManager.IsInRoleAsync(user, "User");
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
                if (isUser && isAdmin)
                {
                    var result = await _signInManager.PasswordSignInAsync(signInDto.username, signInDto.password, false, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("TopicList", "Topic");
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

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Setting()
        {
            return View();
        }

        [Authorize(Roles = "User")]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> MailConfirm()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MailConfirm(EmailConfirm emailConfirm)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _user.CheckEmailConfirmToken(emailConfirm.userName, emailConfirm.confirmationCode);
                if (Result == true)
                {
                    var theUser = await _userManager.FindByNameAsync(emailConfirm.userName);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(theUser);
                    await _userManager.ConfirmEmailAsync(theUser, token);
                    return RedirectToAction("SignIn");
                }
                ModelState.AddModelError("", "User is not found with this code!");
            }
            return View(emailConfirm);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PasswordReset(PasswordReset passwordReset)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(passwordReset.email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found!");
                    return View(passwordReset);
                }
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetUrl = Url.Action("PasswordRecover", "Account", new { userid = user.Id, code = resetToken }, protocol: HttpContext.Request.Scheme);
                ms.SendMail("Mysterious Encyclopedia - Password Reset", $"Hello\nYou can reset your password by clicking the below link\n{resetUrl}", "emrekaratas076@gmail.com", passwordReset.email);
                ModelState.AddModelError("", "Your password recovery link is sent, please check your email");
            }
            return View(passwordReset);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordRecover(string userid, string code)
        {
            TempData["userid"] = userid;
            TempData["token"] = code;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PasswordRecover(PasswordRecover passwordRecover)
        {
            var userid = TempData["userid"];
            var token = TempData["token"];

            if (ModelState.IsValid)
            {
                if (userid == null || token == null)
                    ModelState.AddModelError("", "Credentials are not valid!");
                else
                {
                    var user = await _userManager.FindByIdAsync(userid.ToString());
                    var result = await _userManager.ResetPasswordAsync(user, token.ToString(), passwordRecover.password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
            }
            return View(passwordRecover);
        }


        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UserList(int page = 1)
        {
            var users = await _user.GetAllAsync();
            return View(users.ToPagedList(page, 10));
        }

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
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
