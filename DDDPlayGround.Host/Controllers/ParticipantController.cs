using Microsoft.AspNetCore.Mvc;

namespace DDDPlayGround.Host.Controllers;

public class ParticipantController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
