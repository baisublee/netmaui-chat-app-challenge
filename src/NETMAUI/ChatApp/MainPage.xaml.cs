using Microsoft.Maui.Controls;
using ChatApp.ViewModels;

namespace ChatApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainMenuViewModel();
        }

        private void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                // Cast the selected item to your data model
                var selectedCharacter = e.CurrentSelection[0] as Character;

                // Navigate to different content based on the selected character
                if (selectedCharacter != null)
                {
                    // Update ContentFrame with the selected character's view
                    // ContentFrame.Content = new Label
                    // {
                    //     Text = $"Selected: {selectedCharacter.Name}",
                    //     HorizontalOptions = LayoutOptions.Center,
                    //     VerticalOptions = LayoutOptions.Center
                    // };
                }
            }
        }
    }
}
