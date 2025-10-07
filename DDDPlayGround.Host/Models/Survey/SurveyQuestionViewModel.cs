namespace DDDPlayGround.Host.Models.Survey;

public class SurveyQuestionViewModel
{
    public int Id { get; set; }
    public int QuestionTypeId { get; set; }
    public string QuestionTextEn { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
}
