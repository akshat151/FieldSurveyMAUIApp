using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FieldSurveyMAUIApp.ViewModels
{
    /// <summary>
    /// Base view model class that implements property change notification mechanism
    /// and provides common functionality for view models.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Tracks whether the view model is currently busy processing an operation.
        /// </summary>
        private bool _isBusy = false;

        /// <summary>
        /// Stores the title associated with the view.
        /// </summary>
        private string _title = string.Empty;

        /// <summary>
        /// Stores any error message that needs to be displayed.
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// Gets or sets a value indicating whether the view model is busy.
        /// When set, also updates the IsNotBusy property.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set 
            { 
                SetProperty(ref _isBusy, value);
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the view model is not busy.
        /// This is the inverse of IsBusy.
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// Gets or sets the title of the view.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Gets or sets the error message to be displayed.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. 
        /// This parameter is optional and can be automatically provided by the compiler.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property value if it has changed and raises the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingStore">Reference to the backing field of the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The name of the property. 
        /// This parameter is optional and can be automatically provided by the compiler.</param>
        /// <returns>True if the value was changed, false if the existing value matched the desired value.</returns>
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Executes an asynchronous action while managing the busy state and error handling.
        /// Sets the IsBusy flag to true during execution and captures any exceptions in the ErrorMessage property.
        /// </summary>
        /// <param name="action">The asynchronous action to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected async Task ExecuteWithBusyIndicator(Func<Task> action)
        {
            if (IsBusy)
                return;

            try
            {
                ErrorMessage = string.Empty;
                IsBusy = true;
                await action();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
