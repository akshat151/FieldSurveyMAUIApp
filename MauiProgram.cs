using FieldSurveyMAUIApp.Services;
using FieldSurveyMAUIApp.Services.Interfaces;
using FieldSurveyMAUIApp.ViewModels;
using FieldSurveyMAUIApp.Views;
using Microsoft.Extensions.Logging;

namespace FieldSurveyMAUIApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register HttpClient as a singleton
            builder.Services.AddSingleton<HttpClient>();

            // Register Services
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ISurveyService, SurveyService>();

            // Register ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>(provider => new HomeViewModel(
                provider.GetRequiredService<ISurveyService>(),
                provider.GetRequiredService<IDispatcher>(),
                provider.GetRequiredService<IAuthService>()
            ));
            builder.Services.AddTransient<SurveyViewModel>(provider => new SurveyViewModel(
                provider.GetRequiredService<ISurveyService>(),
                provider.GetRequiredService<IDispatcher>()
            ));
            builder.Services.AddTransient<FilledSurveysViewModel>(provider => new FilledSurveysViewModel(
                provider.GetRequiredService<ISurveyService>(),
                provider.GetRequiredService<IDispatcher>()
            ));

            // Register Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<SurveyPage>();
            builder.Services.AddTransient<FilledSurveysPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
