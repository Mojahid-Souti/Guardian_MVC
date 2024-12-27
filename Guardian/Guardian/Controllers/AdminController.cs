using Guardian.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guardian.Controllers;
[Authorize]
public class AdminController : Controller
{
    // GET: /Admin/Incidents
    public IActionResult Incidents(string statusFilter)
    {
        GuardianContext Guardian = new GuardianContext();
        // Fetch all incidents, optionally filtered by status
        var query = Guardian.Incidents.AsQueryable();
        if (!string.IsNullOrEmpty(statusFilter))
        {
            query = query.Where(i => i.Status == statusFilter);
        }

        var incidents = query.OrderBy(i => i.Status).ToList();

        // Pass the current filter and incidents to the view
        ViewBag.StatusFilter = statusFilter;
        ViewBag.Name = HttpContext.Session.GetString("Name");
        return View(incidents);
    }

    // POST: /Admin/Resolve
    [HttpPost]
    public IActionResult Resolve(int incidentId, string comment)
    {
        GuardianContext Guardian = new GuardianContext();
        var incident = Guardian.Incidents.FirstOrDefault(i => i.IncidentId == incidentId);
        if (incident != null)
        {
            incident.Status = "Resolved";
            incident.Comment = comment;
            incident.ResolvedAt = DateTime.Now;
            Guardian.SaveChanges();
        }
        return RedirectToAction(nameof(Incidents));
    }

    // POST: /Admin/Assign
    [HttpPost]
    public IActionResult Assign(int incidentId, string assignedTo)
    {
        GuardianContext Guardian = new GuardianContext();
        var incident = Guardian.Incidents.FirstOrDefault(i => i.IncidentId == incidentId);
        if (incident != null)
        {
            incident.AssignedTo = assignedTo;
            Guardian.SaveChanges();
        }
        return RedirectToAction(nameof(Incidents));
    }

    // POST: /Admin/Delete
    [HttpPost]
    public IActionResult Delete(int incidentId)
    {
        GuardianContext Guardian = new GuardianContext();
        var incident = Guardian.Incidents.FirstOrDefault(i => i.IncidentId == incidentId);
        if (incident != null)
        {
            Guardian.Incidents.Remove(incident);
            Guardian.SaveChanges();
        }
        return RedirectToAction(nameof(Incidents));
    }
}
