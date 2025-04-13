using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FieldSurveyMAUIApp.Models;
using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace FieldSurveyMAUIApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ISurveyService _surveyService;
        private readonly IDispatcher _dispatcher;
        private readonly IAuthService _authService;

        public ObservableCollection<Survey> Surveys { get; } = new ObservableCollection<Survey>();
        public ICommand LoadSurveysCommand { get; }
        public ICommand SelectSurveyCommand { get; }
        public ICommand LogoutCommand { get; }

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

        private async Task LoadSurveysAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                var surveys = await _surveyService.GetSurveysAsync();
                
                _dispatcher.Dispatch(() =>
                {
                    Surveys.Clear();
                    
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
                            
                            Surveys.Add(survey);
                        }
                    }
                });
            });
        }

        private async Task OnSurveySelected(Survey survey)
        {
            if (survey == null)
                return;

            // Navigate to survey page and pass survey ID as parameter
            await Shell.Current.GoToAsync($"survey?id={survey.Id}");
        }

        private async Task LogoutAsync()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//login");
        }
    }
}