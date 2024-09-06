using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ChatApp.Models;
using System.Diagnostics;

namespace ChatApp.Services
{
    public class ChatPersistService
    {
        private static ChatPersistService _instance;
        public static ChatPersistService Instance => _instance ??= new ChatPersistService();

        private readonly SQLiteAsyncConnection _database;
        private const string SelectedCharacterTable = "SelectedCharacter";
        private const string SelectedCharacterIdKey = "SelectedCharacterId";

        // Constructor to initialize SQLite connection
        private ChatPersistService()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ai/character-chat", "ChatApp.db3");
            Debug.WriteLine($"Database path: {dbPath}");
            _database = new SQLiteAsyncConnection(dbPath);

            // Create tables for messages and selected character
            _database.CreateTableAsync<MessageFlat>().Wait();
            _database.CreateTableAsync<SelectedCharacter>().Wait();
        }

        // Save a message to the database
        public async Task SaveMessageAsync(Message message)
        {
            var messageFlat = MessageFlat.FromMessage(message);
            await _database.InsertAsync(messageFlat);
        }

        // Get messages for a specific user (Character ID)
        public async Task<List<Message>> GetMessagesForUserAsync(string userId)
        {
            var messageFlats = await _database.Table<MessageFlat>().Where(m => m.CharacterId == userId).ToListAsync();
            var messages = new List<Message>();

            foreach (var messageFlat in messageFlats)
            {

                messages.Add(MessageFlat.ToMessage(messageFlat));
            }

            return messages;
        }

        // **NEW** Get all messages from the database
        public async Task<List<Message>> LoadAllMessagesAsync()
        {
            var messageFlats = await _database.Table<MessageFlat>().ToListAsync();
            var messages = new List<Message>();

            foreach (var messageFlat in messageFlats)
            {

                messages.Add(MessageFlat.ToMessage(messageFlat));
            }

            return messages;

        }
        // Save the selected character's ID
        // Save the selected character's ID
        // Save the selected character's ID
        public async Task SaveSelectedCharacterId(string characterId)
        {
            // First, check if a record already exists
            var existingCharacter = await _database.Table<SelectedCharacter>().FirstOrDefaultAsync();

            if (existingCharacter != null)
            {
                // Update the existing record with the new character ID
                existingCharacter.CharacterId = characterId;

                // Ensure that the record has a valid primary key before updating
                await _database.UpdateAsync(existingCharacter);
            }
            else
            {
                // If no record exists, insert a new one
                var selectedCharacter = new SelectedCharacter
                {
                    CharacterId = characterId
                };
                await _database.InsertAsync(selectedCharacter);
            }
        }        // Get the persisted selected character's ID
        public async Task<string> GetPersistedSelectedCharacterId()
        {
            var selectedCharacter = await _database.Table<SelectedCharacter>().FirstOrDefaultAsync();
            return selectedCharacter?.CharacterId;
        }

        // Model class for SelectedCharacter in SQLite
        public class SelectedCharacter
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; } // This is the primary key

            public string CharacterId { get; set; }
        }
    }
}