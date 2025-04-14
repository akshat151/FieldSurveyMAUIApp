using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FieldSurveyMAUIApp.Services.Interfaces;

namespace FieldSurveyMAUIApp.Services
{
    /// <summary>
    /// Provides authentication functionality for the application including login and logout operations.
    /// Communicates with a backend API to authenticate users.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://akshat15.pythonanywhere.com";
        private bool _isLoggedIn;

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="httpClient">HttpClient for making API requests</param>
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets a value indicating whether the user is currently logged in.
        /// </summary>
        public bool IsLoggedIn => _isLoggedIn;

        /// <summary>
        /// Authenticates a user with the backend API using the provided credentials.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="password">The user's password</param>
        /// <returns>True if login was successful; otherwise, false.</returns>
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new
                {
                    username,
                    password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/login", content);
                
                if (response.IsSuccessStatusCode)
                {
                    _isLoggedIn = true;
                    return true;
                }
                
                _isLoggedIn = false;
                return false;
            }
            catch (Exception)
            {
                _isLoggedIn = false;
                return false;
            }
        }

        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        public void Logout()
        {
            _isLoggedIn = false;
        }
    }
}