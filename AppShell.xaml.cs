using FieldSurveyMAUIApp.Views;

namespace FieldSurveyMAUIApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            // Register routes for navigation
            Routing.RegisterRoute("survey", typeof(SurveyPage));
            Routing.RegisterRoute("responses", typeof(FilledSurveysPage));
        }
    }
}
