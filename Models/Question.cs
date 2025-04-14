using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FieldSurveyMAUIApp.Models
{
    /// <summary>
    /// Represents a question in a survey with various properties defining its behavior and content.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Unique identifier for the question.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The text content of the question that will be displayed to users.
        /// </summary>
        [JsonPropertyName("questionText")]
        public string Text { get; set; }
        
        /// <summary>
        /// Defines the type of question which determines the UI control and validation rules.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }
        
        /// <summary>
        /// Indicates whether an answer to this question is mandatory.
        /// </summary>
        [JsonPropertyName("isRequired")]
        public bool Required { get; set; }
        
        /// <summary>
        /// Collection of available options for Choice type questions.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<string> ChoiceOptions { get; set; } = new List<string>();
        
        /// <summary>
        /// Gets a list of Choice objects derived from ChoiceOptions.
        /// This property is not serialized to JSON.
        /// </summary>
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

    /// <summary>
    /// Defines the different types of questions that can be presented in a survey.
    /// </summary>
    public enum QuestionType
    {
        /// <summary>Free text response</summary>
        Text,
        /// <summary>Numerical response</summary>
        Number,
        /// <summary>Date selection</summary>
        Date,
        /// <summary>Selection from predefined options</summary>
        Choice,
        /// <summary>Geographic location data</summary>
        Location
    }

    /// <summary>
    /// Represents a selectable option for Choice type questions with separate value and display text.
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// The underlying value of the choice option.
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// The display text shown to the user for the choice option.
        /// </summary>
        public string Label { get; set; }
    }
}