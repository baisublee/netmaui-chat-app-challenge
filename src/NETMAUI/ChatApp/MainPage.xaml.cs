using Microsoft.Maui.Controls;
using System;
using ChatApp.ViewModels;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace ChatApp
{
    public partial class MainPage : ContentPage
    {

        public MainPageViewModel ViewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainPageViewModel();
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
                var characters = await CAAService.Instance.GetCharactersAsync();

                if (characters != null && characters.Count > 0)
                {
                    // Set the characters in the ViewModel
                    var CharacterList = new List<CharacterViewModel>(
                        characters.Select(c => new CharacterViewModel
                        {
                            Id = c.Id,
                            Name = c.CharacterName,
                            Image = "default_character_image.png", // Assuming a default image or map the actual image here
                            Description = c.Description.Personality // You can expand this as needed
                        })
                    );

                    ViewModel.Characters = new ObservableCollection<CharacterViewModel>(CharacterList);

                    // Set the left menu character (e.g., the first character)
                    ViewModel.SelectedCharacter = ViewModel.Characters.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading characters: {ex.Message}");
                Debug.WriteLine($"Error loading characters: {ex.Message}");
            }
        }


        private void OnCharacterTapped(object sender, EventArgs e)
        {
            var tappedCharacter = (sender as View).BindingContext as CharacterViewModel;
            if (tappedCharacter != null)
            {
                DisplayAlert("Character Tapped", $"You tapped on {tappedCharacter.Name}", "OK");
                // Add additional logic here to handle character selection
                Debug.WriteLine("Main Page OnCharacterTapped: " + tappedCharacter.Name);

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
    }
}
