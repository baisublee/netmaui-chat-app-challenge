using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using ChatApp.Services;

namespace ChatApp.Services
{
    public class MessageService
    {
        private static MessageService _instance;
        public static MessageService Instance => _instance ??= new MessageService();

        // Internal storage for characters and their messages
        private Dictionary<Character, ObservableCollection<Message>> _characterMessages;
        private List<Character> _charactersInMemory;  // Store characters from the network in memory

        private MessageService()
        {
            _characterMessages = new Dictionary<Character, ObservableCollection<Message>>();
            _charactersInMemory = new List<Character>(); // Initialize empty list for characters
        }

        // Method to load characters from the network and store them in memory
        public async Task InitializeCharactersAsync()
        {
            var characters = await CAAService.Instance.GetCharactersAsync(); // Fetch characters from the network
            _charactersInMemory = characters; // Store characters in memory
        }

        // Get characters from memory
        public List<Character> GetCharactersInMemory()
        {
            return _charactersInMemory;
        }

        // Load messages from the database on app start and match them with characters from the network
        public async Task LoadDataFromDatabaseAsync()
        {
            var messages = await ChatPersistService.Instance.LoadAllMessagesAsync();

            foreach (var message in messages)
            {
                var character = _charactersInMemory.Find(c => c.Id == message.UserId); // Match with in-memory characters
                if (character != null)
                {
                    AddMessageToCharacter(character, message, persist: false);
                }
            }
        }

        // Add a message to a character and persist it in the database
        public async void AddMessageToCharacter(Character character, Message message, bool persist = true)
        {
            if (_characterMessages.ContainsKey(character))
            {
                _characterMessages[character].Add(message);
            }
            else
            {
                var messages = new ObservableCollection<Message> { message };
                _characterMessages[character] = messages;
            }

            // Persist the message in the database
            if (persist)
            {
                await ChatPersistService.Instance.AddMessageAsync(message);
            }
        }

        public ObservableCollection<Message> GetMessagesForCharacter(Character character)
        {
            if (_characterMessages.ContainsKey(character))
            {
                return _characterMessages[character];
            }

            return new ObservableCollection<Message>();
        }

        // New Method to Create a Character and Update In-Memory List
        public async Task<Character> CreateNewCharacterAsync(object characterRequest)
        {
            try
            {
                // Use CAAService to create a new character
                var createdCharacter = await CAAService.Instance.CreateCharacterAsync(characterRequest);

                // Update the in-memory character list and create greeting message
                if (createdCharacter != null)
                {
                    _charactersInMemory.Add(createdCharacter); // Add to the in-memory list

                    // Create the greeting message for the new character
                    var greetingMessage = new Message
                    {
                        UserId = createdCharacter.Id,  // Assuming 'Id' is the identifier for the character
                        Text = createdCharacter.Description.GreetingMessage ?? "Hello!", // Use greeting message if available
                        Time = DateTime.Now.ToString("HH:mm"),
                        Sender = createdCharacter // The character is the sender of the greeting message
                    };

                    // Add the greeting message to the new character's messages and persist it
                    AddMessageToCharacter(createdCharacter, greetingMessage);

                    // Persist the new message in the database
                    await ChatPersistService.Instance.AddMessageAsync(greetingMessage);
                }

                return createdCharacter;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed (logging, etc.)
                Console.WriteLine($"Error creating character: {ex.Message}");
                return null;
            }
        }

        public async Task SaveAllMessagesAsync()
        {
            foreach (var character in _characterMessages.Keys)
            {
                await ChatPersistService.Instance.SaveMessagesForUserAsync(character, _characterMessages[character]);
            }
        }
    }
}