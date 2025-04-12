using System;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Converters
{
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate NumberTemplate { get; set; }
        public DataTemplate DateTemplate { get; set; }
        public DataTemplate ChoiceTemplate { get; set; }
        public DataTemplate LocationTemplate { get; set; }

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