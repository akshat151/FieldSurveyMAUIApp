using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;

        public App(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            MainPage = new AppShell();
        }
    }
}