using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                _selectedCharacter = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            Characters = new ObservableCollection<CharacterViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CharacterViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } // Image path or URL
        public string Description { get; set; } // Character description or personality
    }
}