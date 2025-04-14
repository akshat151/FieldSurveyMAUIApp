using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace FieldSurveyMAUIApp.ViewModels
{
    /// <summary>
    /// ViewModel for displaying submitted surveys
    /// </summary>
    public class FilledSurveysViewModel : BaseViewModel
    {
        // Service dependencies
        private readonly ISurveyService _surveyService; // Handles survey data operations
        private readonly IDispatcher _dispatcher; // Manages UI thread operations

        // Observable collection to bind to UI
        public ObservableCollection<SurveyResponseViewModel> Responses { get; } = new ObservableCollection<SurveyResponseViewModel>();
        
        // Command to trigger loading of survey responses
        public ICommand LoadResponsesCommand { get; }

        /// <summary>
        /// Constructor - initializes services and commands
        /// </summary>
        /// <param name="surveyService">Service to fetch survey data</param>
        /// <param name="dispatcher">UI thread dispatcher</param>
        public FilledSurveysViewModel(ISurveyService surveyService, IDispatcher dispatcher)
        {
            _surveyService = surveyService;
            _dispatcher = dispatcher;
            Title = "Submitted Surveys";
            LoadResponsesCommand = new Command(async () => await LoadResponsesAsync());
        }

        /// <summary>
        /// Loads survey responses from service and updates the observable collection
        /// </summary>
        private async Task LoadResponsesAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                // Get responses from the service
                var responses = await _surveyService.GetSurveyResponsesAsync();
                
                // Use dispatcher to update UI on main thread
                _dispatcher.Dispatch(() =>
                {
                    Responses.Clear();
                    
                    // Display error message if no responses found
                    if (responses.Count == 0)
                    {
                        ErrorMessage = "No survey responses available.";
                    }
                    else
                    {
                        foreach (var response in responses)
                        {
                            // Create view model for each survey response
                            var responseVM = new SurveyResponseViewModel
                            {
                                SurveyId = response.SurveyId,
                                SubmissionDate = response.SubmissionDate
                            };
                            
                            // Process individual question responses
                            if (response.Responses != null)
                            {
                                foreach (var item in response.Responses)
                                {
                                    // Create view model for each response item
                                    var responseItem = new ResponseItemViewModel
                                    {
                                        QuestionId = item.Key,
                                        Value = item.Value?.ToString() ?? "N/A" // Convert value to string or use N/A if null
                                    };
                                    responseVM.Items.Add(responseItem);
                                }
                            }
                            
                            // Add to main collection
                            Responses.Add(responseVM);
                        }
                    }
                });
            });
        }
    }

    /// <summary>
    /// ViewModel representing a single survey response
    /// </summary>
    public class SurveyResponseViewModel : BaseViewModel
    {
        public string SurveyId { get; set; } // Unique identifier for the survey
        public string SubmissionDate { get; set; } // When the survey was submitted
        public ObservableCollection<ResponseItemViewModel> Items { get; } = new ObservableCollection<ResponseItemViewModel>(); // Individual question responses
    }

    /// <summary>
    /// ViewModel representing a single question response
    /// </summary>
    public class ResponseItemViewModel : BaseViewModel
    {
        public string QuestionId { get; set; } // Identifier for the question
        public string Value { get; set; } // User's response value
    }
}