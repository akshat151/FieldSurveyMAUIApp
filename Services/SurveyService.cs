using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.Services.Interfaces;

namespace FieldSurveyMAUIApp.Services
{
    /// <summary>
    /// Provides functionality to interact with survey data from the backend API.
    /// Handles retrieving surveys, survey details, submitting responses, and fetching previous responses.
    /// </summary>
    public class SurveyService : ISurveyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://akshat15.pythonanywhere.com";
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Initializes a new instance of the SurveyService class.
        /// </summary>
        /// <param name="httpClient">HttpClient for making API requests</param>
        public SurveyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a list of all available surveys from the API.
        /// </summary>
        /// <returns>A list of Survey objects if successful; otherwise, an empty list.</returns>
        public async Task<List<Survey>> GetSurveysAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiSurveyList>($"{_baseUrl}/api/surveys", _jsonOptions);
                return response?.Surveys ?? new List<Survey>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting surveys: {ex.Message}");
                return new List<Survey>();
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific survey.
        /// </summary>
        /// <param name="surveyId">The unique identifier of the survey</param>
        /// <returns>A Survey object containing survey details if successful; otherwise, null.</returns>
        public async Task<Survey> GetSurveyDetailsAsync(string surveyId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Survey>($"{_baseUrl}/api/surveys/{surveyId}", _jsonOptions);
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting survey details: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Submits responses to a survey to the backend API.
        /// </summary>
        /// <param name="surveyId">The unique identifier of the survey being submitted</param>
        /// <param name="responses">List of question responses to submit</param>
        /// <returns>True if submission was successful; otherwise, false.</returns>
        public async Task<bool> SubmitSurveyResponseAsync(string surveyId, List<QuestionResponse> responses)
        {
            try
            {
                // Convert our internal QuestionResponse list to the format expected by the API
                var responseDictionary = new Dictionary<string, object>();
                
                foreach (var res in responses)
                {
                    if (res.LocationData != null)
                    {
                        // Format the timestamp as ISO string for location data
                        res.LocationData.Timestamp = DateTime.UtcNow.ToString("o");
                        responseDictionary[res.QuestionId] = res.LocationData;
                    }
                    else
                    {
                        responseDictionary[res.QuestionId] = res.Value;
                    }
                }

                var payload = new
                {
                    responses = responseDictionary
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/surveys/{surveyId}/responses", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error submitting survey response: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves a list of previously submitted survey responses from the API.
        /// </summary>
        /// <returns>A list of SurveyResponse objects if successful; otherwise, an empty list.</returns>
        public async Task<List<SurveyResponse>> GetSurveyResponsesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiSurveyResponse>($"{_baseUrl}/api/surveyResponses", _jsonOptions);
                return response?.Responses ?? new List<SurveyResponse>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting survey responses: {ex.Message}");
                return new List<SurveyResponse>();
            }
        }
    }
}