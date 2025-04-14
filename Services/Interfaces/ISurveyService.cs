using FieldSurveyMAUIApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FieldSurveyMAUIApp.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for survey-related services in the application.
    /// Implementations handle retrieving, submitting, and managing survey data.
    /// </summary>
    public interface ISurveyService
    {
        /// <summary>
        /// Retrieves a list of all available surveys.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of Survey objects.</returns>
        Task<List<Survey>> GetSurveysAsync();
        
        /// <summary>
        /// Retrieves detailed information about a specific survey.
        /// </summary>
        /// <param name="surveyId">The unique identifier of the survey</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a Survey object with details.</returns>
        Task<Survey> GetSurveyDetailsAsync(string surveyId);
        
        /// <summary>
        /// Submits responses to a survey.
        /// </summary>
        /// <param name="surveyId">The unique identifier of the survey being submitted</param>
        /// <param name="responses">List of question responses to submit</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether submission was successful.</returns>
        Task<bool> SubmitSurveyResponseAsync(string surveyId, List<QuestionResponse> responses);
        
        /// <summary>
        /// Retrieves a list of previously submitted survey responses.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of SurveyResponse objects.</returns>
        Task<List<SurveyResponse>> GetSurveyResponsesAsync();
    }
}