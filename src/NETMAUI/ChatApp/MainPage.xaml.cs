using Microsoft.Maui.Controls;
using System;
using ChatApp.ViewModels;

namespace ChatApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCharacterTapped(object sender, EventArgs e)
        {
            var tappedCharacter = (sender as View).BindingContext as Character;
            if (tappedCharacter != null)
            {
                DisplayAlert("Character Tapped", $"You tapped on {tappedCharacter.Name}", "OK");
                // Add additional logic here to handle character selection
            }
        }

        private void OnCreateNewCharacter(object sender, EventArgs e)
        {
            DisplayAlert("Create New Character", "Create New Character button tapped", "OK");
            // Add logic to navigate to character creation page or show a dialog
        }

        private void OnOpenMusicCreator(object sender, EventArgs e)
        {
            DisplayAlert("Open Music Creator", "Music Creator option tapped", "OK");
            // Add logic to open the music creator page or functionality
        }

        private void OnOpenVideoCreator(object sender, EventArgs e)
        {
            DisplayAlert("Open Video Creator", "Video Creator option tapped", "OK");
            // Add logic to open the video creator page or functionality
        }
    }
}
