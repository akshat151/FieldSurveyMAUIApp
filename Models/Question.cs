using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FieldSurveyMAUIApp.Models
{
    public class Question
    {
        public string Id { get; set; }
        
        [JsonPropertyName("questionText")]
        public string Text { get; set; }
        
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }
        
        [JsonPropertyName("isRequired")]
        public bool Required { get; set; }
        
        [JsonPropertyName("choices")]
        public List<string> ChoiceOptions { get; set; } = new List<string>();
        
        [JsonIgnore]
        public List<Choice> Choices 
        { 
            get 
            {
                var result = new List<Choice>();
                if (ChoiceOptions != null)
                {
                    foreach (var option in ChoiceOptions)
                    {
                        result.Add(new Choice { Value = option, Label = option });
                    }
                }
                return result;
            }
        }
    }

    public enum QuestionType
    {
        Text,
        Number,
        Date,
        Choice,
        Location
    }

    public class Choice
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}