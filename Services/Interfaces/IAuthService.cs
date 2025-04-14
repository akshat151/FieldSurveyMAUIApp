using System.Threading.Tasks;

namespace FieldSurveyMAUIApp.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for authentication services in the application.
    /// Implementations of this interface handle user authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="password">The user's password</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether login was successful.</returns>
        Task<bool> LoginAsync(string username, string password);
        
        /// <summary>
        /// Gets a value indicating whether the user is currently logged in.
        /// </summary>
        bool IsLoggedIn { get; }
        
        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        void Logout();
    }
}