using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class MessageService
    {
        // Singleton instance
        private static MessageService _instance;
        public static MessageService Instance => _instance ??= new MessageService();

        // Internal storage for users and their messages
        private Dictionary<User, ObservableCollection<Message>> _userMessages;

        // Private constructor for singleton pattern
        private MessageService()
        {
            _userMessages = new Dictionary<User, ObservableCollection<Message>>();
        }

        // Method to update the entire list of User and reconstruct message lists
        public void UpdateUserList(IEnumerable<User> userList)
        {
            // Clear the old list of users and messages
            _userMessages.Clear();

            // Add each user with an empty ObservableCollection<Message>
            foreach (var user in userList)
            {
                _userMessages[user] = new ObservableCollection<Message>();
            }
        }

        // Method to add a new user and its initial greeting message
        public void AddUserWithGreeting(User newUser)
        {
            if (!_userMessages.ContainsKey(newUser))
            {
                var messages = new ObservableCollection<Message>();

                // Add the greeting message
                var greetingMessage = new Message
                {
                    Sender = newUser,
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = newUser.Description?.GreetingMessage ?? "Hello!"
                };
                messages.Add(greetingMessage);

                // Add the user and their messages to the dictionary
                _userMessages[newUser] = messages;
            }
        }

        // Method to get the message list for a specific user
        public ObservableCollection<Message> GetMessagesForUser(User user)
        {
            if (_userMessages.ContainsKey(user))
            {
                return _userMessages[user];
            }

            return new ObservableCollection<Message>(); // Return an empty collection if no messages are found
        }

        // Method to add a message to a specific user
        public void AddMessageToUser(User user, Message message)
        {
            if (_userMessages.ContainsKey(user))
            {
                _userMessages[user].Add(message);
            }
            else
            {
                // If the user is not found, add the user with the message
                var messages = new ObservableCollection<Message> { message };
                _userMessages[user] = messages;
            }
        }

        // Method to retrieve the entire user-message map (for testing or administrative purposes)
        public Dictionary<User, ObservableCollection<Message>> GetUserMessageMap()
        {
            return _userMessages;
        }
    }
}