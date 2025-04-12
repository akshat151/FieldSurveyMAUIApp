using FieldSurveyMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadSurveysCommand.Execute(null);
        }

        private async void OnViewResponsesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("responses");
        }
    }
}