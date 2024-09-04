using ChatApp.Models;
using ChatApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp.ViewModels
{
    public class DetailViewModel : ViewModelBase, INotifyPropertyChanged
    {
        User _user;
        ObservableCollection<Message> _messages;

        // private CharacterViewModel _SelectedCharacterViewModel;
        // public CharacterViewModel SelectedCharacterViewModel
        // {
        //     get => _SelectedCharacterViewModel;
        //     set
        //     {
        //         _SelectedCharacterViewModel = value;
        //         // OnPropertyChanged(); // Notify when User changes
        //         // LoadMessagesForUser(_SelectedCharacterViewModel); // Load messages for the new user
        //         if (_SelectedCharacterViewModel != null)
        //         {
        //             Console.WriteLine($"DetailView for {_SelectedCharacterViewModel.Name}");
        //             // Update UI or perform actions based on the character
        //             User = new User
        //             {
        //                 Id = _SelectedCharacterViewModel.Id,
        //                 AvatarImage = _SelectedCharacterViewModel.Image,
        //                 CharacterName = _SelectedCharacterViewModel.Name,
        //             };
        //             LoadMessagesForUser(User);
        //         }



        //     }
        // }

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand => new Command(OnBack);

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Message message)
            {
                User = message.Sender;
                // Messages = new ObservableCollection<Message>(MessageService.Instance.GetMessages(User));
                Messages = MessageService.Instance.GetMessagesForUser(User);
            }

            return base.InitializeAsync(navigationData);

        }

        // public event PropertyChangedEventHandler PropertyChanged;

        // protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        // {
        //     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // }
        // Method to update messages for a selected user
        public void LoadMessagesForUser(User userParam)
        {
            if (userParam == null)
            {
                User = null;
                Messages = new ObservableCollection<Message>();
            }
            else
            {
                User = userParam;
                Messages = MessageService.Instance.GetMessagesForUser(userParam);
            }

        }

        void OnBack()
        {
            // NavigationService.Instance.NavigateBackAsync();
        }
    }
}