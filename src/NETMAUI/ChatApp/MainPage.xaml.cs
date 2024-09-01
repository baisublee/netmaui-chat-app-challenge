using Microsoft.Maui.Controls;
using System;
using ChatApp.ViewModels;
using System.Diagnostics;

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
