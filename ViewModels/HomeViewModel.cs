using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.Services.Interfaces;

namespace FieldSurveyMAUIApp.ViewModels
{
    /// <summary>
    /// ViewModel for the home page displaying available surveys
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        // Services used by the ViewModel
        private readonly ISurveyService _surveyService;   // Service for fetching survey data
        private readonly IDispatcher _dispatcher;         // Dispatcher for UI thread operations
        private readonly IAuthService _authService;       // Service for authentication operations

        // Properties
        /// <summary>
        /// Collection of available surveys displayed to the user
        /// </summary>
        public ObservableCollection<Survey> Surveys { get; } = new ObservableCollection<Survey>();
        
        // Commands
        /// <summary>
        /// Command to load available surveys from the service
        /// </summary>
        public ICommand LoadSurveysCommand { get; }
        
        /// <summary>
        /// Command to handle selection of a survey from the list
        /// </summary>
        public ICommand SelectSurveyCommand { get; }
        
        /// <summary>
        /// Command to handle user logout
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// Constructor initializes services and commands
        /// </summary>
        public HomeViewModel(ISurveyService surveyService, IDispatcher dispatcher, IAuthService authService)
        {
            _surveyService = surveyService;
            _dispatcher = dispatcher;
            _authService = authService;
            Title = "Available Disaster Reporting Surveys";
            LoadSurveysCommand = new Command(async () => await LoadSurveysAsync());
            SelectSurveyCommand = new Command<Survey>(async (survey) => await OnSurveySelected(survey));
            LogoutCommand = new Command(async () => await LogoutAsync());
        }

        /// <summary>
        /// Loads available surveys from the service and updates the UI collection
        /// </summary>
        private async Task LoadSurveysAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                // Get surveys from the service
                var surveys = await _surveyService.GetSurveysAsync();
                
                _dispatcher.Dispatch(() =>
                {
                    // Clear existing surveys
                    Surveys.Clear();
                    
                    // Show message if no surveys are available
                    if (surveys.Count == 0)
                    {
                        ErrorMessage = "No disaster surveys available.";
                    }
                    else
                    {
                        foreach (var survey in surveys)
                        {
                            // Set default image if none provided by the API
                            if (string.IsNullOrEmpty(survey.ImageUrl))
                            {
                                survey.ImageUrl = "dotnet_bot.png";
                            }
                            
                            // Add survey to the observable collection
                            Surveys.Add(survey);
                        }
                    }
                });
            });
        }

        /// <summary>
        /// Handles the selection of a survey and navigates to the survey details page
        /// </summary>
        /// <param name="survey">The selected survey</param>
        private async Task OnSurveySelected(Survey survey)
        {
            if (survey == null)
                return;

            // Navigate to survey page and pass survey ID as parameter
            await Shell.Current.GoToAsync($"survey?id={survey.Id}");
        }

        /// <summary>
        /// Logs out the current user and navigates to the login page
        /// </summary>
        private async Task LogoutAsync()
        {
            // Log out the user through the authentication service
            _authService.Logout();
            
            // Navigate back to the login page
            await Shell.Current.GoToAsync("//login");
        }
    }
}