using ChatApp.Views;

namespace ChatApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// MainPage = new NavigationPage(new HomeView());
		MainPage = new AppShell();
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);

        const int newWidth = 1220;
		const int newHeight = 600;

        window.X = 100;
		window.Y = 100;
        window.Width = newWidth;
		window.Height = newHeight;
        window.MinimumHeight = newHeight;        
		window.MinimumWidth = newWidth;
		window.MinimumHeight = newHeight;        
		window.MaximumWidth = newWidth;

        return window;
    }

}