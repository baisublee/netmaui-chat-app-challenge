using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ChatApp.Models;

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
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ChatApp.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            // Create tables for messages and selected character
            _database.CreateTableAsync<Message>().Wait();
            _database.CreateTableAsync<SelectedCharacter>().Wait();
        }

        // Save a message to the database
        public async Task SaveMessageAsync(Message message)
        {
            await _database.InsertAsync(message);
        }

        // Get messages for a specific user (Character ID)
        public async Task<List<Message>> GetMessagesForUserAsync(string userId)
        {
            return await _database.Table<Message>().Where(m => m.CharacterId == userId).ToListAsync();
        }

        // **NEW** Get all messages from the database
        public async Task<List<Message>> LoadAllMessagesAsync()
        {
            return await _database.Table<Message>().ToListAsync();
        }
        // Save the selected character's ID
        public async Task SaveSelectedCharacterId(string characterId)
        {
            var selectedCharacter = new SelectedCharacter
            {
                CharacterId = characterId
            };

            await _database.InsertOrReplaceAsync(selectedCharacter);
        }

        // Get the persisted selected character's ID
        public async Task<string> GetPersistedSelectedCharacterId()
        {
            var selectedCharacter = await _database.Table<SelectedCharacter>().FirstOrDefaultAsync();
            return selectedCharacter?.CharacterId;
        }

        // Model class for SelectedCharacter in SQLite
        public class SelectedCharacter
        {
            [PrimaryKey]
            public string CharacterId { get; set; }
        }
    }
}