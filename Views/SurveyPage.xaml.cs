using FieldSurveyMAUIApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Views
{
    public partial class SurveyPage : ContentPage
    {
        private readonly SurveyViewModel _viewModel;
        
        public SurveyPage(SurveyViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }
    }
}