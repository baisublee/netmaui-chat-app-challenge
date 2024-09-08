using Microsoft.Maui.Controls;
using System;
using ChatApp.ViewModels;
using System.Diagnostics;
using System.Collections.ObjectModel;
using ChatApp.Views; // Assuming DetailView is in this namespace
using ChatApp.Services;

namespace ChatApp
{
    public partial class MainPage : ContentPage, IMainPageActions
    {

        public MainPageViewModel ViewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainPageViewModel(this);
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Load characters when the page appears
            await LoadCharactersAsync();
        }

        private async Task LoadCharactersAsync()
        {
            try
            {
                // Get the list of characters from CAAService
                // var characters = await CAAService.Instance.GetCharactersAsync();
                await MessageService.Instance.InitializeCharactersAsync();
                var characters = MessageService.Instance.GetCharactersInMemory();

                var CharacterList = new List<CharacterViewModel>();
                if (characters != null && characters.Count > 0)
                {
                    // iterate through the characters and create a CharacterViewModel for each
                    foreach (var c in characters)
                    {
                        CharacterList.Add(new CharacterViewModel
                        {
                            Id = c.Id,
                            Name = c.CharacterName,
                            Image = "rachel_image.png", // Assuming a default image or map the actual image here
                        });
                    }

                }

                CharacterList.Add(new CharacterViewModel
                {
                    Id = "create",
                    Name = "Create",
                    Image = "create.png",
                    IsCreateItem = true,
                });

                var selectedCharacterId = await ChatPersistService.Instance.GetPersistedSelectedCharacterId();
                CharacterViewModel selectedCharacter = null;
                if (selectedCharacterId != null)
                {
                    selectedCharacter = CharacterList.Find(c => c.Id == selectedCharacterId);
                }
                if (selectedCharacter == null)
                {
                    selectedCharacter = CharacterList.FirstOrDefault();
                }

                ViewModel.Characters = new ObservableCollection<CharacterViewModel>(CharacterList);
                ViewModel.SelectedCharacter = selectedCharacter;

                await HandleCharacterSelection(ViewModel.SelectedCharacter);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading characters: {ex.Message}");
                Debug.WriteLine($"Error loading characters: {ex.Message}");
            }
        }


        // This method is triggered when a character is tapped in the UI
        private async void OnCharacterTapped(object sender, EventArgs e)
        {
            var tappedCharacter = (e as TappedEventArgs)?.Parameter as CharacterViewModel;
            await HandleCharacterSelection(tappedCharacter);
        }

        private async Task HandleCharacterSelection(CharacterViewModel tappedCharacter)
        {
            if (tappedCharacter != null)
            {
                // Handle the character selection in MainPage.xaml.cs
                Console.WriteLine($"Character Tapped: {tappedCharacter.Name}");
                Debug.WriteLine($"Character Tapped: {tappedCharacter.Name}");

                if (tappedCharacter.Id != "create")
                {
                    Debug.WriteLine($"Persist Selected Character: {tappedCharacter.Name}");
                    await ChatPersistService.Instance.SaveSelectedCharacterId(tappedCharacter.Id);
                }

                // Now set the SelectedCharacter in the ViewModel
                ViewModel.SelectedCharacter = tappedCharacter;

                // Load the appropriate page into the ContentFrame based on the selection
                if (tappedCharacter.IsCreateItem)
                {
                    // Load the CreationPage if IsCreateItem is true
                    // ContentFrame.Content = new CreationPage();
                    var creationPage = new CreationPage ();
                    
                    var wrapper = new ContentView
                    {
                        Content = creationPage.Content
                    };
                    ContentFrame.Content = wrapper;
                }
                else
                {
                    CharacterSelectionService.Instance.SetSelectedCharacter(tappedCharacter);

                    // Load the DetailView and pass the selected CharacterViewModel
                    var detailView = new DetailView();
                    var wrapper = new ContentView
                    {
                        Content = detailView.Content
                    };
                    // Assign the wrapper to the ContentFrame
                    ContentFrame.Content = wrapper;

                    // Set the BindingContext after adding to the visual tree
                    // Manually trigger the UI update method
                    // detailView.UpdateUIBasedOnCharacter();
                }
            }
        }

        private void OnCreateNewCharacter(object sender, EventArgs e)
        {
            Debug.WriteLine("Main Page OnCreateNewCharacter");
            DisplayAlert("Create New Character", "Create New Character button tapped", "OK");
            // Add logic to navigate to character creation page or show a dialog
        }

        private void OnOpenMusicCreator(object sender, EventArgs e)
        {
            Debug.WriteLine("Main Page OnOpenMusicCreator");
            DisplayAlert("Open Music Creator", "Music Creator option tapped", "OK");
            // Add logic to open the music creator page or functionality
        }

        private void OnOpenVideoCreator(object sender, EventArgs e)
        {
            Debug.WriteLine("Main Page OnOpenVideoCreator");
            DisplayAlert("Open Video Creator", "Video Creator option tapped", "OK");
            // Add logic to open the video creator page or functionality
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Main Page OnBackButtonClicked");
            // Handle back button click
            // Navigate to the previous page or perform any other action
            // Navigation.PopAsync(); // Example: Navigating back in the navigation stack
        }

        // Method from IMainPageActions interface for showing alert
        public async Task ShowAlert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        // Method from IMainPageActions interface for adding a new character
        public async Task AddNewCharacter(string characterName)
        {
            // Add new character logic (you can update this as per your app logic)
            Debug.WriteLine($"Adding new character: {characterName}");

            // For example, you could call a service to add the new character and update the list:
            var newCharacter = new CharacterViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = characterName,
                Image = "new_character_image.png"
            };

            ViewModel.Characters.Add(newCharacter);
            await ChatPersistService.Instance.SaveSelectedCharacterId(newCharacter.Id);
        }        
    }
}
