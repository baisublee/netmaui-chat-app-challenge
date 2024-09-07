using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ChatApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace ChatApp.ViewModels
{
    public class CreationPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public string TestName { get; set; } = "Hello, World! from test name";
        
        // Fields bound to the form
        public string CharacterName { get; set; } = "Hello, World!";
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
        public ICommand CreateCommand { get; }
        public ICommand EditImageCommand { get; }

        public CreationPageViewModel()
        {
            // Initialize commands
            CreateCommand = new Command(OnCreateCharacter);
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

        private void OnCreateCharacter()
        {
            // Add logic to send the form data to the API
            Debug.WriteLine("Creating character...");
        }

        private void OnEditImage()
        {
            // Handle image editing logic
            Debug.WriteLine("Editing image...");
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}