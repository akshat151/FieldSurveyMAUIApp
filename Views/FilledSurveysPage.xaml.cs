using FieldSurveyMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Views
{
    public partial class FilledSurveysPage : ContentPage
    {
        private readonly FilledSurveysViewModel _viewModel;
        
        public FilledSurveysPage(FilledSurveysViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadResponsesCommand.Execute(null);
        }
    }
}