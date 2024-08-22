namespace ChatApp;
public partial class OnboardingWelcome : ContentPage
{
    public OnboardingWelcome()
    {
        InitializeComponent();
    }

    private async void OnGetStartedButtonClicked(object sender, EventArgs e)
    {
        // Navigate to the next page or perform any action you want
        // await Navigation.PushAsync(new NextPage()); // Replace 'NextPage' with your actual next page
        await Shell.Current.GoToAsync("//OnboardingSelectCharacter");
    }
}
