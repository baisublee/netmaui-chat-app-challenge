using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Input; // Add this using directive

namespace ChatApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CharacterViewModel> _characters;
        private CharacterViewModel _selectedCharacter;

        public ObservableCollection<CharacterViewModel> Characters
        {
            get => _characters;
            set
            {
                _characters = value;
                OnPropertyChanged();
            }
        }

        public CharacterViewModel SelectedCharacter
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

        public MainPageViewModel()
        {
            Characters = new ObservableCollection<CharacterViewModel>();
            // Command for creating a new character
            CreateNewCharacterCommand = new Command(OnCreateNewCharacter);
            OpenMusicCreatorCommand = new Command(OpenMusicCreator);
            OpenVideoCreatorCommand = new Command(OpenVideoCreator); // Initialize the command
            OnCharacterTappedCommand = new Command<CharacterViewModel>(OnCharacterTapped); // Initialize the command
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand LoadCharactersCommand { get; set; }
        public ICommand CreateNewCharacterCommand { get; set; }
        public ICommand OpenMusicCreatorCommand { get; set; }
        public ICommand OpenVideoCreatorCommand { get; set; } // Define the missing property
        public ICommand OnCharacterTappedCommand { get; set; } // Define the missing property


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

        private void OnCharacterTapped(CharacterViewModel character)
        {
            // Logic to handle when a character is tapped
            SelectedCharacter = character;
            Debug.WriteLine($"Main Menu View Tapped character: {character?.Name}");
        }

        private void OnCharacterSelected(CharacterViewModel character)
        {
            Debug.WriteLine($" Main Menu View Selected character: {character?.Name}");
            // Logic to handle when a character is selected
            if (character != null)
            {
                // For example, update the main content area based on the selected character
            }
        }

    }

    public class CharacterViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public string Image { get; set; }

        public string Id { get; set; }


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
}