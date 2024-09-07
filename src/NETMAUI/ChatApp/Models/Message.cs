using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ChatApp.Models;
using ChatApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;
using ChatApp.ViewModels;
using Microsoft.Maui.Controls;

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

        public bool IsGreetingMessage { get; set; } = false;

        public bool IsOtherAppPromotionMesaage { get; set; } = false;

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
            CreateMusicCommand = new Command(OnCreateMusic);
            SearchDocumentsCommand = new Command(OnSearchDocuments);
        }

        // Method to get the current time in epoch milliseconds
        private int GetCurrentEpochTimeInMilliseconds()
        {
            return (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public ICommand CreateMusicCommand { get; }

        // Command for the second button (Search Documents)
        public ICommand SearchDocumentsCommand { get; }

        // Method called when the first button is clicked
        private void OnCreateMusic()
        {
            // Do something when the "Create instrumental music" button is clicked
            Debug.WriteLine("Create Music Button Clicked");
            Console.WriteLine("Create Music Button Clicked");
            // DisplayAlert("Open Music Creator", "Music Creator option tapped", "OK");
            // You can navigate to a new page, trigger a service, etc.
        }

        // Method called when the second button is clicked
        private void OnSearchDocuments()
        {
            // Do something when the "Search everything in gram" button is clicked
            Debug.WriteLine("Create Music Button Clicked");
            Console.WriteLine("Search Documents Button Clicked");
            // DisplayAlert("Open Video Creator", "Video Creator option tapped", "OK");
            // You can navigate to a new page, trigger a service, etc.
        }

    }
}