using Microsoft.AspNetCore.Mvc;
using DDDPlayGround.Host.Models.Survey;

namespace DDDPlayGround.Host.Controllers;

public class ParticipantController : Controller
{
    [HttpGet("/Participant/Index/{id:int}")]
    public IActionResult Index(int id)
    {
        // For now, return static data; later, replace with real backend fetch by id
        var model = new SurveyViewModel
        {
            Name = "Customer Satisfaction Survey",
            Pages = new List<SurveyPageViewModel>
            {
                new SurveyPageViewModel
                {
                    PageNumber = 1,
                    Questions = new List<SurveyQuestionViewModel>
                    {
                        new SurveyQuestionViewModel{ Id = 1, QuestionTypeId = 1, QuestionTextEn = "Are you satisfied with our service?", IsMandatory = true },
                        new SurveyQuestionViewModel{ Id = 2, QuestionTypeId = 1, QuestionTextEn = "Would you recommend us to others?", IsMandatory = false }
                    }
                }
            }
        };

        return View(model);
    }
}
