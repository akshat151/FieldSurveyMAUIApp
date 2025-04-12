using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace FieldSurveyMAUIApp.ViewModels
{
    [QueryProperty(nameof(SurveyId), "id")]
    public class SurveyViewModel : BaseViewModel
    {
        private readonly ISurveyService _surveyService;
        private readonly IDispatcher _dispatcher;
        private string _surveyId;
        private Survey _survey;

        public string SurveyId
        {
            get => _surveyId;
            set
            {
                SetProperty(ref _surveyId, value);
                if (!string.IsNullOrEmpty(value))
                {
                    LoadSurveyAsync().ConfigureAwait(false);
                }
            }
        }

        public Survey Survey
        {
            get => _survey;
            set => SetProperty(ref _survey, value);
        }

        public ObservableCollection<QuestionViewModel> Questions { get; } = new ObservableCollection<QuestionViewModel>();
        
        public ICommand SubmitCommand { get; }
        public ICommand GetLocationCommand { get; }

        public SurveyViewModel(ISurveyService surveyService, IDispatcher dispatcher)
        {
            _surveyService = surveyService;
            _dispatcher = dispatcher;
            
            SubmitCommand = new Command(async () => await SubmitSurveyAsync());
            GetLocationCommand = new Command<QuestionViewModel>(async (questionVM) => await GetLocationAsync(questionVM));
        }

        private async Task LoadSurveyAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                var survey = await _surveyService.GetSurveyDetailsAsync(SurveyId);

                _dispatcher.Dispatch(() =>
                {
                    if (survey == null)
                    {
                        ErrorMessage = "Failed to load survey.";
                        return;
                    }

                    Survey = survey;
                    Title = survey.Title;
                    Questions.Clear();

                    foreach (var question in survey.Questions)
                    {
                        Questions.Add(new QuestionViewModel(question));
                    }
                });
            });
        }

        private async Task GetLocationAsync(QuestionViewModel questionVM)
        {
            try
            {
                var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync(
                    new Microsoft.Maui.Devices.Sensors.GeolocationRequest
                    {
                        DesiredAccuracy = Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(15)
                    });

                if (location != null)
                {
                    questionVM.LocationData = new LocationData
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Accuracy = (double)location.Accuracy,
                        Timestamp = location.Timestamp.DateTime.ToString("o") // ISO 8601 format
                    };

                    questionVM.Answer = $"Lat: {location.Latitude:F6}, Long: {location.Longitude:F6}";
                }
                else
                {
                    questionVM.Answer = "Failed to get location";
                }
            }
            catch (Exception ex)
            {
                questionVM.Answer = $"Error: {ex.Message}";
            }
        }

        private async Task SubmitSurveyAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                // Validate required questions
                var requiredQuestions = Questions.Where(q => q.Question.Required && string.IsNullOrEmpty(q.Answer));
                if (requiredQuestions.Any())
                {
                    ErrorMessage = "Please answer all required questions.";
                    return;
                }

                // Build responses
                var responses = new List<QuestionResponse>();
                foreach (var questionVM in Questions)
                {
                    // Handle different types of questions
                    var response = new QuestionResponse
                    {
                        QuestionId = questionVM.Question.Id,
                        LocationData = null
                    };

                    // Special handling for different question types
                    switch (questionVM.Question.Type)
                    {
                        case QuestionType.Number:
                            // Try to parse as number for correct JSON serialization
                            if (double.TryParse(questionVM.Answer, out double numValue))
                            {
                                response.Value = numValue.ToString();
                            }
                            else
                            {
                                response.Value = questionVM.Answer;
                            }
                            break;
                        
                        case QuestionType.Location:
                            response.LocationData = questionVM.LocationData;
                            break;
                        
                        default:
                            response.Value = questionVM.Answer;
                            break;
                    }
                    
                    responses.Add(response);
                }

                bool success = await _surveyService.SubmitSurveyResponseAsync(SurveyId, responses);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Survey response submitted successfully.", "OK");
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    ErrorMessage = "Failed to submit survey response.";
                }
            });
        }
    }

    public class QuestionViewModel : BaseViewModel
    {
        private string _answer;
        private LocationData _locationData;

        public Question Question { get; }

        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        public LocationData LocationData
        {
            get => _locationData;
            set => SetProperty(ref _locationData, value);
        }

        public QuestionViewModel(Question question)
        {
            Question = question;
        }
    }
}