using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FieldSurveyMAUIApp.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.ViewModels
{
    /// <summary>
    /// ViewModel for the login page, handles user authentication
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private string _username;
        private string _password;

        /// <summary>
        /// Gets or sets the username for login
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// Gets or sets the password for login
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// Command that executes the login process
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Constructor initializes the login view model with required services
        /// </summary>
        /// <param name="authService">Service for authentication operations</param>
        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Login";
            LoginCommand = new Command(async () => await LoginAsync());
        }

        /// <summary>
        /// Handles the login process asynchronously
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task LoginAsync()
        {
            await ExecuteWithBusyIndicator(async () =>
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Username and password are required.";
                    return;
                }

                // Attempt to authenticate user
                bool result = await _authService.LoginAsync(Username, Password);

                if (result)
                {
                    // Navigate to Home page on successful login
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    // Display error message for failed login
                    ErrorMessage = "Invalid username or password.";
                }
            });
        }
    }
}