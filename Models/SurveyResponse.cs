using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FieldSurveyMAUIApp.Models
{
    /// <summary>
    /// Represents a completed response to a survey, containing answers to the survey questions.
    /// </summary>
    public class SurveyResponse
    {
        /// <summary>
        /// The identifier of the survey to which this response corresponds.
        /// </summary>
        [JsonPropertyName("surveyId")]
        public string SurveyId { get; set; }
        
        /// <summary>
        /// The date and time when the survey was submitted.
        /// </summary>
        [JsonPropertyName("submittedAt")]
        public string SubmissionDate { get; set; }
        
        /// <summary>
        /// Collection of question responses, where the key is the question ID and the value is the response data.
        /// </summary>
        [JsonPropertyName("responses")]
        public Dictionary<string, object> Responses { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// API response model for retrieving a list of submitted survey responses.
    /// </summary>
    public class ApiSurveyResponse
    {
        /// <summary>
        /// List of survey responses returned from the API.
        /// </summary>
        public List<SurveyResponse> Responses { get; set; } = new List<SurveyResponse>();
    }

    /// <summary>
    /// Represents an answer to an individual question within a survey for internal app use.
    /// </summary>
    public class QuestionResponse
    {
        /// <summary>
        /// The identifier of the question being answered.
        /// </summary>
        public string QuestionId { get; set; }
        
        /// <summary>
        /// The response value provided by the user.
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Location data associated with this response, if applicable.
        /// </summary>
        public LocationData LocationData { get; set; }
    }

    /// <summary>
    /// Contains geographic location information captured for location-based questions.
    /// </summary>
    public class LocationData
    {
        /// <summary>
        /// The latitude coordinate in decimal degrees.
        /// </summary>
        [JsonPropertyName("Latitude")]
        public double Latitude { get; set; }
        
        /// <summary>
        /// The longitude coordinate in decimal degrees.
        /// </summary>
        [JsonPropertyName("Longitude")]
        public double Longitude { get; set; }
        
        /// <summary>
        /// The accuracy of the location measurement in meters.
        /// </summary>
        [JsonPropertyName("Accuracy")]
        public double Accuracy { get; set; }
        
        /// <summary>
        /// The timestamp when the location data was captured.
        /// </summary>
        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; }
    }
}