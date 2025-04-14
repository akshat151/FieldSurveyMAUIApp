using System;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Converters
{
    /// <summary>
    /// Template selector that dynamically selects the appropriate data template based on question type.
    /// This enables rendering different UI elements for different question types in a survey form.
    /// </summary>
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Template for rendering text-based questions (free text input)
        /// </summary>
        public DataTemplate TextTemplate { get; set; }
        
        /// <summary>
        /// Template for rendering number-based questions (numeric input)
        /// </summary>
        public DataTemplate NumberTemplate { get; set; }
        
        /// <summary>
        /// Template for rendering date-based questions (date picker)
        /// </summary>
        public DataTemplate DateTemplate { get; set; }
        
        /// <summary>
        /// Template for rendering choice-based questions (dropdown/picker)
        /// </summary>
        public DataTemplate ChoiceTemplate { get; set; }
        
        /// <summary>
        /// Template for rendering location-based questions (map or coordinate input)
        /// </summary>
        public DataTemplate LocationTemplate { get; set; }

        /// <summary>
        /// Determines which template to use based on the question type.
        /// </summary>
        /// <param name="item">The data object for which to select a template (should be a QuestionViewModel)</param>
        /// <param name="container">The parent object which will contain the templated UI</param>
        /// <returns>The appropriate DataTemplate based on the question type</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is QuestionViewModel questionViewModel)
            {
                // Select the appropriate template based on the question type
                return questionViewModel.Question.Type switch
                {
                    QuestionType.Text => TextTemplate,
                    QuestionType.Number => NumberTemplate,
                    QuestionType.Date => DateTemplate,
                    QuestionType.Choice => ChoiceTemplate,
                    QuestionType.Location => LocationTemplate,
                    _ => TextTemplate // Default to TextTemplate if type is unknown
                };
            }

            // If item is not a QuestionViewModel, default to TextTemplate
            return TextTemplate;
        }
    }
}