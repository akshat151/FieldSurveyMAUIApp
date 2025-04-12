using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FieldSurveyMAUIApp.Models
{
    public class Survey
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
        
        [JsonPropertyName("questions")]
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public class ApiSurveyList
    {
        [JsonPropertyName("surveys")]
        public List<Survey> Surveys { get; set; } = new List<Survey>();
    }
}