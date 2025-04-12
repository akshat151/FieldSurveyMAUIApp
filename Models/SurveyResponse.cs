using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FieldSurveyMAUIApp.Models
{
    public class SurveyResponse
    {
        [JsonPropertyName("surveyId")]
        public string SurveyId { get; set; }
        
        [JsonPropertyName("submittedAt")]
        public string SubmissionDate { get; set; }
        
        [JsonPropertyName("responses")]
        public Dictionary<string, object> Responses { get; set; } = new Dictionary<string, object>();
    }

    public class ApiSurveyResponse
    {
        public List<SurveyResponse> Responses { get; set; } = new List<SurveyResponse>();
    }

    // For internal app use to track responses per question
    public class QuestionResponse
    {
        public string QuestionId { get; set; }
        public string Value { get; set; }
        public LocationData LocationData { get; set; }
    }

    public class LocationData
    {
        [JsonPropertyName("Latitude")]
        public double Latitude { get; set; }
        
        [JsonPropertyName("Longitude")]
        public double Longitude { get; set; }
        
        [JsonPropertyName("Accuracy")]
        public double Accuracy { get; set; }
        
        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; }
    }
}