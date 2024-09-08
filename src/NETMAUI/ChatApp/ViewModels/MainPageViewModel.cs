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

        private readonly IMainPageActions _mainPageActions;

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

        public MainPageViewModel(IMainPageActions mainPageActions)
        {
            
             _mainPageActions = mainPageActions;

            Characters = new ObservableCollection<CharacterViewModel>();
            // Command for creating a new character
            CreateNewCharacterCommand = new Command(OnCreateNewCharacter);
            OpenMusicCreatorCommand = new Command(OpenMusicCreator);
            OpenVideoCreatorCommand = new Command(OpenVideoCreator); // Initialize the command
            OnCharacterTappedCommand = new Command<CharacterViewModel>(OnCharacterTapped); // Initialize the command

            InitCreationPageViewModel();
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

        /* This part comes from Creation Page View Model */

        // Fields bound to the form
        public string CharacterName { get; set; }
        public string SelectedGender { get; set; }
        public string SelectedPronouns { get; set; }
        public string SelectedStageOfLife { get; set; }
        public string CoreDescription { get; set; }
        public string GreetingMessage { get; set; }

        // Gender, Pronouns, and Stage of Life options from API spec
        public ObservableCollection<string> GenderOptions { get; set; }
        public ObservableCollection<string> PronounsOptions { get; set; }
        public ObservableCollection<string> StageOfLifeOptions { get; set; }

        // Commands
        public ICommand CreateCommand { get; set; }
        public ICommand EditImageCommand { get; set; }

        public void InitCreationPageViewModel()
        {
            // Initialize commands
            CreateCommand = new Command(async (object obj) => await OnCreateCharacter());
            EditImageCommand = new Command(OnEditImage);

            // Initialize lists with enum values from the API
            GenderOptions = new ObservableCollection<string>
            {
                "Male", "Female", "Transgender", "Non-binary/non-conforming", "Other"
            };

            PronounsOptions = new ObservableCollection<string>
            {
                "He/him", "She/her", "They/Them", "Other"
            };

            StageOfLifeOptions = new ObservableCollection<string>
            {
                "Childhood", "Adolescence", "Young Adult", "Adulthood", "Middle age", "Senior age/elderly", "Other"
            };

        }

        private async Task OnCreateCharacter()
        {
            // Add logic to send the form data to the API
            Debug.WriteLine("Creating character...");
            // If CharacterName or CoreDescription is empty, show Alert saying "Please fill in the required fields" 
            if (string.IsNullOrEmpty(CharacterName) || string.IsNullOrEmpty(CoreDescription))
            {
                Debug.WriteLine("Please fill in the required fields");
                
                // Show alert
                await _mainPageActions.ShowAlert("Create Character", "Please fill in the required fields", "OK");
        
                return;
            }
        
            // Add logic to send the form data to the API
            Debug.WriteLine("Creating character...");
            
            // var requestObject = {
            //     CharacterName: CharacterName,
            // }
        
        
        
            
        }

        private void OnEditImage()
        {
            // Handle image editing logic
            Debug.WriteLine("Editing image...");
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

        public CharacterViewModel Clone()
        {
            return new CharacterViewModel
            {
                Name = this.Name,
                Image = this.Image,
                Id = this.Id,
                IsSelected = this.IsSelected,
                IsCreateItem = this.IsCreateItem
            };
        }
    }
}