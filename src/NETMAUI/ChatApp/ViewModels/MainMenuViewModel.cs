using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.ComponentModel;

namespace ChatApp.ViewModels
{
    public class Character : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public string Image { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public bool IsCreateItem { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
                new Character { Name = "Olivia", Image = "olivia_image.jpeg" },
                new Character { Name = "Chris", Image = "chris_image.png" },
                new Character { Name = "Create", Image = "create.png", IsCreateItem = true }
            };

            // Command for creating a new character
            CreateNewCharacterCommand = new Command(OnCreateNewCharacter);

            // Command for opening gram music creator
            OpenMusicCreatorCommand = new Command(OpenMusicCreator);

            // Command for opening gram video creator
            OpenVideoCreatorCommand = new Command(OpenVideoCreator);

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
                if (_selectedCharacter != value)
                {
                    if (_selectedCharacter != null)
                        _selectedCharacter.IsSelected = false;
                    _selectedCharacter = value;
                    if (_selectedCharacter != null)
                        _selectedCharacter.IsSelected = true;
                    OnPropertyChanged();
                    // Handle the character selection
                    OnCharacterSelected(value);
                }
            }
        }

        public ICommand CreateNewCharacterCommand { get; }
        public ICommand OpenMusicCreatorCommand { get; }
        public ICommand OpenVideoCreatorCommand { get; }
        public ICommand OnCharacterTappedCommand { get; }

        private void OnCreateNewCharacter()
        {
            // Logic for creating a new character
            // For example, you could open a dialog to enter character details
            Debug.WriteLine("Main Menu View CreateNewCharacter");
        }

        private void OpenMusicCreator()
        {
            // Logic to open the gram music creator
            Debug.WriteLine("Main Menu View OpenMusicCreator");
        }

        private void OpenVideoCreator()
        {
            // Logic to open the gram video creator
            Debug.WriteLine("Main Menu View OpenVideoCreator");
        }

        private void OnCharacterTapped(Character character)
        {
            // Logic to handle when a character is tapped
            SelectedCharacter = character;
            Debug.WriteLine($"Main Menu View Tapped character: {character?.Name}");
        }

        private void OnCharacterSelected(Character character)
        {
            Debug.WriteLine($" Main Menu View Selected character: {character?.Name}");
            // Logic to handle when a character is selected
            if (character != null)
            {
                // For example, update the main content area based on the selected character
            }
        }
    }
}
