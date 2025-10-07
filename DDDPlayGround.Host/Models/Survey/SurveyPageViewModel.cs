namespace DDDPlayGround.Host.Models.Survey;

public class SurveyPageViewModel
{
    public int PageNumber { get; set; }
    public List<SurveyQuestionViewModel> Questions { get; set; } = new();
}
