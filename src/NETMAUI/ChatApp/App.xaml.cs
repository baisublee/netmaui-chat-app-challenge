using ChatApp.Views;
using ChatApp.Services; 

namespace ChatApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// MainPage = new NavigationPage(new HomeView());
		MainPage = new AppShell();
	}

	protected override async void OnStart()
	{
		// Initialize characters from the network and store them in memory
		await MessageService.Instance.InitializeCharactersAsync();

		// Load persisted messages from the database and map them to the in-memory characters
		await MessageService.Instance.LoadDataFromDatabaseAsync();

		// Load selected character from the database
		var characters = MessageService.Instance.GetCharactersInMemory();
		await CharacterSelectionService.Instance.LoadSelectedCharacterAsync(characters);
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