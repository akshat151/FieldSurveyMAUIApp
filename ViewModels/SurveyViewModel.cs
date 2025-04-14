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
    /// <summary>
    /// ViewModel for displaying and handling survey details and responses
    /// Receives survey ID via Shell navigation
    /// </summary>
    [QueryProperty(nameof(SurveyId), "id")]
    public class SurveyViewModel : BaseViewModel
    {
        private readonly ISurveyService _surveyService;
        private readonly IDispatcher _dispatcher;
        private string _surveyId;
        private Survey _survey;

        /// <summary>
        /// Survey ID property that triggers loading the survey when set
        /// </summary>
        public string SurveyId
        {
            get => _surveyId;
            set
            {
                SetProperty(ref _surveyId, value);
                if (!string.IsNullOrEmpty(value))
                {
                    // Load survey when ID is set
                    LoadSurveyAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Survey details object
        /// </summary>
        public Survey Survey
        {
            get => _survey;
            set => SetProperty(ref _survey, value);
        }

        /// <summary>
        /// Collection of question view models for the survey
        /// </summary>
        public ObservableCollection<QuestionViewModel> Questions { get; } = new ObservableCollection<QuestionViewModel>();
        
        /// <summary>
        /// Command to submit the completed survey
        /// </summary>
        public ICommand SubmitCommand { get; }
        
        /// <summary>
        /// Command to get geolocation for location questions
        /// </summary>
        public ICommand GetLocationCommand { get; }

        /// <summary>
        /// Constructor for SurveyViewModel
        /// </summary>
        /// <param name="surveyService">Service for survey operations</param>
        /// <param name="dispatcher">UI dispatcher for thread-safe UI updates</param>
        public SurveyViewModel(ISurveyService surveyService, IDispatcher dispatcher)
        {
            _surveyService = surveyService;
            _dispatcher = dispatcher;
            
            SubmitCommand = new Command(async () => await SubmitSurveyAsync());
            GetLocationCommand = new Command<QuestionViewModel>(async (questionVM) => await GetLocationAsync(questionVM));
        }

        /// <summary>
        /// Loads survey details from the service
        /// </summary>
        private async Task LoadSurveyAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                // Get survey details from service
                var survey = await _surveyService.GetSurveyDetailsAsync(SurveyId);

                _dispatcher.Dispatch(() =>
                {
                    if (survey == null)
                    {
                        ErrorMessage = "Failed to load survey.";
                        return;
                    }

                    // Update UI with survey data
                    Survey = survey;
                    Title = survey.Title;
                    Questions.Clear();

                    // Create view models for each question
                    foreach (var question in survey.Questions)
                    {
                        Questions.Add(new QuestionViewModel(question));
                    }
                });
            });
        }

        /// <summary>
        /// Gets the current device location for location questions
        /// </summary>
        /// <param name="questionVM">The question view model to update with location data</param>
        private async Task GetLocationAsync(QuestionViewModel questionVM)
        {
            try
            {
                // Request device location with medium accuracy
                var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync(
                    new Microsoft.Maui.Devices.Sensors.GeolocationRequest
                    {
                        DesiredAccuracy = Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(15)
                    });

                if (location != null)
                {
                    // Store location data object for submission
                    questionVM.LocationData = new LocationData
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Accuracy = (double)location.Accuracy,
                        Timestamp = location.Timestamp.DateTime.ToString("o") // ISO 8601 format
                    };

                    // Display coordinates in the answer field
                    questionVM.Answer = $"Lat: {location.Latitude:F6}, Long: {location.Longitude:F6}";
                }
                else
                {
                    questionVM.Answer = "Failed to get location";
                }
            }
            catch (Exception ex)
            {
                // Handle location errors
                questionVM.Answer = $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Submits the completed survey responses
        /// </summary>
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

                // Build responses from question view models
                var responses = new List<QuestionResponse>();
                foreach (var questionVM in Questions)
                {
                    var response = new QuestionResponse
                    {
                        QuestionId = questionVM.Question.Id,
                        LocationData = null
                    };

                    // Handle different question types appropriately
                    switch (questionVM.Question.Type)
                    {
                        case QuestionType.Number:
                            // Parse number values for proper serialization
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
                            // Include location data for location questions
                            response.LocationData = questionVM.LocationData;
                            break;
                        
                        default:
                            // Text and other types
                            response.Value = questionVM.Answer;
                            break;
                    }
                    
                    responses.Add(response);
                }

                // Submit responses to service
                bool success = await _surveyService.SubmitSurveyResponseAsync(SurveyId, responses);

                if (success)
                {
                    // Show success message and navigate home
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

    /// <summary>
    /// ViewModel for individual survey questions
    /// </summary>
    public class QuestionViewModel : BaseViewModel
    {
        private string _answer;
        private LocationData _locationData;

        /// <summary>
        /// The question model
        /// </summary>
        public Question Question { get; }

        /// <summary>
        /// The user's answer to the question
        /// </summary>
        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        /// <summary>
        /// Location data for location-type questions
        /// </summary>
        public LocationData LocationData
        {
            get => _locationData;
            set => SetProperty(ref _locationData, value);
        }

        /// <summary>
        /// Constructor for QuestionViewModel
        /// </summary>
        /// <param name="question">The question model</param>
        public QuestionViewModel(Question question)
        {
            Question = question;
        }
    }
}