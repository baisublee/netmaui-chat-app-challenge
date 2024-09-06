using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatApp.Models
{
    public class Message : INotifyPropertyChanged
    {
        private User _sender;
        private string _text;
        private string _time;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // CharacterId to store the associated character's ID
        public string CharacterId { get; set; }

        // CreatedAt to store message creation time (default to current epoch time in milliseconds)
        public int CreatedAt { get; set; }

        public string SenderId { get; set; } = null;

        public User Sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
                if (value != null)
                {
                    SenderId = value.Id;
                }
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
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

        // Constructor to initialize CreatedAt with the current epoch time
        public Message()
        {
            CreatedAt = GetCurrentEpochTimeInMilliseconds();
        }

        // Method to get the current time in epoch milliseconds
        private int GetCurrentEpochTimeInMilliseconds()
        {
            return (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }
}