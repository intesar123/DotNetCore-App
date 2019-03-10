using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyCoreAppAuth.Models;

namespace MyCoreAppAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        // GET: api/<controller>
        private IConfiguration _config;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _config = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }
       
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user,string returnUrl="")
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

            if (result.Succeeded)
            {
                // var appUser = _userManager.Users.SingleOrDefault(r => r.Email == Login.Email);
                //return await GenerateJwtToken(Login.Email, appUser);
                //IActionResult response = Unauthorized();
                //var user = new User().AuthenticateUser(Login);
                //if (user != null)
                //{
                //    var tokenString = GenerateJSONWebToken(user);
                //    response = Ok(new { token = tokenString });
                //}
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            return Content("Invalid Login");
        }
        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User model,string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var user = new Microsoft.AspNetCore.Identity.IdentityUser { UserName=model.UserName }; //identity user .netcore
                var result = await _userManager.CreateAsync(user,model.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user,false);
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout(User user, string returnUrl = "")
        {
            await _signInManager.SignOutAsync();
            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
    }
}