using Guardian.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guardian.Controllers;

[Authorize]
public class UserController : Controller
{
    
    public IActionResult Dashboard()
    {
        GuardianContext dbContext = new GuardianContext();

        // Fetch data for Intrusion Detection Trends
        var packetEntries = dbContext.PacketEntries
            .GroupBy(p => p.Timestamp.Date)
            .Select(g => new
            {
                Date = g.Key, // Keep as DateTime
                Detected = g.Count(),
                Blocked = g.Count(e => e.ThreatLevel == "Low")
            })
            .OrderBy(e => e.Date)
            .ToList();

        // Handle case where there is no data
        if (packetEntries == null || !packetEntries.Any())
        {
            ViewBag.IntrusionTrends = new List<object>(); // Pass an empty list
            ViewBag.ErrorMessage = "No data available.";
            ViewBag.Name = HttpContext.Session.GetString("Name");
        }
        else
        {
            // Format the date on the server-side
            var chartData = packetEntries.Select(p => new
            {
                Date = p.Date.ToString("MMM dd"), // Format the date
                Detected = p.Detected,
                Blocked = p.Blocked
            }).ToList();

            ViewBag.IntrusionTrends = chartData; // Pass data to the view
        }

        // Fetch data for Intrusion Types Distribution
        var intrusionTypes = dbContext.PacketEntries
            .GroupBy(p => p.Attack)
            .Select(g => new
            {
                Type = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(e => e.Count)
            .ToList();

        // Handle case where there is no data
        if (intrusionTypes == null || !intrusionTypes.Any())
        {
            ViewBag.IntrusionTypes = new List<object>(); // Pass an empty list
        }
        else
        {
            ViewBag.IntrusionTypes = intrusionTypes; // Pass intrusion types data to the view
        }

        // Pass additional data to the view
        ViewBag.packets = from packet in dbContext.PacketEntries select packet;
        ViewBag.packetno = (from packet in dbContext.PacketEntries select packet).Count();
        ViewBag.Name = HttpContext.Session.GetString("Name");

        return View();
    }
    [HttpPost]
    public IActionResult DeleteAccount()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        using (var context = new GuardianContext())
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                // Delete the user account
                context.Users.Remove(user);
                context.SaveChanges();

                // Clear the session and log out
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Authentication");
            }
        }

        return RedirectToAction(nameof(Settings));
    }

    public IActionResult Settings()
    {
        ViewData["Title"] = "Settings";

        // Retrieve the user's ID from the session
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication"); // Redirect to login if not logged in
        }

        // Fetch user info from database
        using (var context = new GuardianContext())
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            ViewBag.Name = HttpContext.Session.GetString("Name");
            return View(user); 
        }
    }

    [HttpPost]
    public IActionResult UpdateInfo(string fullName, string emailAddress)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        using (var context = new GuardianContext())
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.FullName = fullName;
                HttpContext.Session.SetString("Name",fullName);
                user.EmailAddress = emailAddress;
                context.SaveChanges();
                TempData["InfoMessage"] = "Info updated successfully.";
            }
        }

        return RedirectToAction(nameof(Settings));
    }

    [HttpPost]
    public IActionResult UpdatePassword(string oldPassword, string newPassword)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        using (GuardianContext context = new GuardianContext())
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                // Verify old password
                if (user.Password != oldPassword)
                {
                    TempData["Error"] = "Old password is incorrect.";
                    return RedirectToAction(nameof(Settings));
                }

                // Update password
                user.Password = newPassword;
                context.SaveChanges();
                TempData["PassMessage"] = "Password updated successfully.";
            }
        }

        return RedirectToAction(nameof(Settings));
    }
}