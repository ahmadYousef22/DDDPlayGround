namespace DDDPlayGround.Host.Models.Survey;

public class SurveyViewModel
{
    public string Name { get; set; } = string.Empty;
    public List<SurveyPageViewModel> Pages { get; set; } = new();
}
