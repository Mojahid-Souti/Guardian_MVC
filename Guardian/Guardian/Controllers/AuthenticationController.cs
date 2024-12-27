using System.Security.Claims;
using Guardian.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Guardian.Controllers;

public class AuthenticationController : Controller
{

  
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Email and password are required.";
            return View();
        }
        GuardianContext Guardian = new GuardianContext();
        var user = Guardian.Users.FirstOrDefault(u => u.EmailAddress == email && u.Password == password);
        if (user != null)
        {
            // Issue authentication cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            HttpContext.Session.SetString("Name", user.FullName);
            HttpContext.Session.SetInt32("UserId", user.UserId);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Error = "Invalid credentials.";
            return View();
        }
    }

    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(string Name, string email, string password)
    {
        
        GuardianContext Guardian = new GuardianContext();
        User user = new User();
        user.FullName = Name;
        user.EmailAddress = email;
        user.Password = password;
        Guardian.Users.Add(user);
        Guardian.SaveChanges();
        TempData["SuccessfulAddition"] = $"{Name}'s Credentials Have Been Succefully Registered";
        return RedirectToAction("Login", "Authentication");
        
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login", "Authentication");
    }
    
}