using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using controlRateSystem.Models.ViewModels;

namespace controlRateSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Реєстрація
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("UserProfile");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        //Логін
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl ?? "/" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                        return Redirect(model.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Невірний логін або пароль.");
            }
            return View(model);
        }

        //Вихід
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        //Профіль
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new UserProfileModel
            {
                Email = user.Email,
                NewPassword = string.Empty,
                ConfirmPassword = string.Empty
            };
            return View(model);
        }

        //Оновлення профілю
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfile(UserProfileModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                // Оновлення електронної пошти
                if (model.Email != user.Email)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email; // також оновлюємо логін
                    var emailResult = await _userManager.UpdateAsync(user);
                    if (!emailResult.Succeeded)
                    {
                        foreach (var error in emailResult.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }

                // Оновлення паролю
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (!passwordResult.Succeeded)
                    {
                        foreach (var error in passwordResult.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    TempData["Message"] = "Дані успішно оновлено.";
                    return RedirectToAction("UserProfile");
                }
            }

            return View(model);
        }
    }
}
