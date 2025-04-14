using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FieldSurveyMAUIApp.Models
{
    /// <summary>
    /// Represents a survey containing multiple questions that can be presented to users.
    /// </summary>
    public class Survey
    {
        /// <summary>
        /// Unique identifier for the survey.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// The title of the survey displayed to users.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        /// <summary>
        /// Detailed description of the survey's purpose and content.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        /// <summary>
        /// URL to an image associated with the survey for display purposes.
        /// </summary>
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
        
        /// <summary>
        /// Collection of questions that make up this survey.
        /// </summary>
        [JsonPropertyName("questions")]
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    /// <summary>
    /// API response model for retrieving a list of available surveys.
    /// </summary>
    public class ApiSurveyList
    {
        /// <summary>
        /// List of surveys returned from the API.
        /// </summary>
        [JsonPropertyName("surveys")]
        public List<Survey> Surveys { get; set; } = new List<Survey>();
    }
}