using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FieldSurveyMAUIApp.Services.Interfaces;

namespace FieldSurveyMAUIApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://akshat15.pythonanywhere.com";
        private bool _isLoggedIn;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool IsLoggedIn => _isLoggedIn;

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

        public void Logout()
        {
            _isLoggedIn = false;
        }
    }
}