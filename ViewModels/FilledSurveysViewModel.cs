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
    public class FilledSurveysViewModel : BaseViewModel
    {
        private readonly ISurveyService _surveyService;
        private readonly IDispatcher _dispatcher;

        public ObservableCollection<SurveyResponseViewModel> Responses { get; } = new ObservableCollection<SurveyResponseViewModel>();
        public ICommand LoadResponsesCommand { get; }

        public FilledSurveysViewModel(ISurveyService surveyService, IDispatcher dispatcher)
        {
            _surveyService = surveyService;
            _dispatcher = dispatcher;
            Title = "Submitted Surveys";
            LoadResponsesCommand = new Command(async () => await LoadResponsesAsync());
        }

        private async Task LoadResponsesAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                var responses = await _surveyService.GetSurveyResponsesAsync();
                
                _dispatcher.Dispatch(() =>
                {
                    Responses.Clear();
                    
                    if (responses.Count == 0)
                    {
                        ErrorMessage = "No survey responses available.";
                    }
                    else
                    {
                        foreach (var response in responses)
                        {
                            // Create view models for each response
                            var responseVM = new SurveyResponseViewModel
                            {
                                SurveyId = response.SurveyId,
                                SubmissionDate = response.SubmissionDate
                            };
                            
                            // Convert dictionary to list of response items
                            if (response.Responses != null)
                            {
                                foreach (var item in response.Responses)
                                {
                                    var responseItem = new ResponseItemViewModel
                                    {
                                        QuestionId = item.Key,
                                        Value = item.Value?.ToString() ?? "N/A"
                                    };
                                    responseVM.Items.Add(responseItem);
                                }
                            }
                            
                            Responses.Add(responseVM);
                        }
                    }
                });
            });
        }
    }

    public class SurveyResponseViewModel : BaseViewModel
    {
        public string SurveyId { get; set; }
        public string SubmissionDate { get; set; }
        public ObservableCollection<ResponseItemViewModel> Items { get; } = new ObservableCollection<ResponseItemViewModel>();
    }

    public class ResponseItemViewModel : BaseViewModel
    {
        public string QuestionId { get; set; }
        public string Value { get; set; }
    }
}