namespace FieldSurveyMAUIApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Shell.Current.GoToAsync("//login");
    }
}

