using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FieldSurveyMAUIApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy = false;
        private string _title = string.Empty;
        private string _errorMessage;

        public bool IsBusy
        {
            get => _isBusy;
            set 
            { 
                SetProperty(ref _isBusy, value);
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !IsBusy;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

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