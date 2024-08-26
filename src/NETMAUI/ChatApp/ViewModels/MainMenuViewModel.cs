using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ChatApp.ViewModels
{
    public class Character
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class MainMenuViewModel : BindableObject
    {
        private ObservableCollection<Character> _characters;
        private Character _selectedCharacter;

        public MainMenuViewModel()
        {
            // Sample characters
            Characters = new ObservableCollection<Character>
            {
                new Character { Name = "Rachel", Image = "rachel_image.png" },
                new Character { Name = "Olivia", Image = "olivia_image.png" },
                new Character { Name = "Chris", Image = "chris_image.png" }
            };

            // Command for creating a new character
            CreateNewCharacterCommand = new Command(OnCreateNewCharacter);

            // Command for character tap gesture
            OnCharacterTappedCommand = new Command<Character>(OnCharacterTapped);
        }

        public ObservableCollection<Character> Characters
        {
            get => _characters;
            set
            {
                _characters = value;
                OnPropertyChanged();
            }
        }

        public Character SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                OnPropertyChanged();
                // Handle the character selection
                OnCharacterSelected(value);
            }
        }

        public ICommand CreateNewCharacterCommand { get; }
        public ICommand OnCharacterTappedCommand { get; }

        private void OnCreateNewCharacter()
        {
            // Logic for creating a new character
            // For example, you could open a dialog to enter character details
        }

        private void OnCharacterTapped(Character character)
        {
            // Logic to handle when a character is tapped
            SelectedCharacter = character;
        }

        private void OnCharacterSelected(Character character)
        {
            // Logic to handle when a character is selected
            if (character != null)
            {
                // For example, update the main content area based on the selected character
            }
        }
    }
}
