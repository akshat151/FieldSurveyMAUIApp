using System.Threading.Tasks;

namespace FieldSurveyMAUIApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        bool IsLoggedIn { get; }
        void Logout();
    }
}