namespace ChatApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		CheckOnboarding();
	}

	private async void CheckOnboarding()
	{
		// bool isOnboardingFinished = !Preferences.ContainsKey("IsOnboardingFinished");
		// if (isOnboardingFinished)
		// {
		// 	Preferences.Set("IsOnboardingFinished", true);
		 	await GoToAsync("//MainPage");
		// }
		// else
		// {
		//	await GoToAsync("//MainPage");
		// }
	}
}
