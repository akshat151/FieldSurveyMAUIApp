using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Login";
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Username and password are required.";
                    return;
                }

                bool result = await _authService.LoginAsync(Username, Password);

                if (result)
                {
                    // Navigate to Home page on successful login
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                }
            });
        }
    }
}