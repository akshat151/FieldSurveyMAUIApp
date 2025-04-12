using FieldSurveyMAUIApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FieldSurveyMAUIApp.Services.Interfaces
{
    public interface ISurveyService
    {
        Task<List<Survey>> GetSurveysAsync();
        Task<Survey> GetSurveyDetailsAsync(string surveyId);
        Task<bool> SubmitSurveyResponseAsync(string surveyId, List<QuestionResponse> responses);
        Task<List<SurveyResponse>> GetSurveyResponsesAsync();
    }
}