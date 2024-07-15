using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp.Models
{
    // public class Message
    // {
    //     public User Sender { get; set; }
    //     public string Text { get; set; }
    //     public string Time { get; set; }
    // }

    public class Message : INotifyPropertyChanged
    {
        private User _sender;
        private string _text;
        private string _time;

        public User Sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                // OnPropertyChanged();
                // OnPropertyChanged(nameof(Text));
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}