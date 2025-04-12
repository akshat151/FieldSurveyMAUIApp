using FieldSurveyMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}