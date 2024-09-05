using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class ChatPersistService
    {
        private static ChatPersistService _instance;
        public static ChatPersistService Instance => _instance ??= new ChatPersistService();

        private SQLiteAsyncConnection _database;

        private const string DatabaseFilename = "ChatApp.db3";

        private ChatPersistService()
        {
            string dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabaseAsync().Wait();
        }

        private async Task InitializeDatabaseAsync()
        {
            // Only create tables for Message and SelectedCharacter
            await _database.CreateTableAsync<Message>();
            await _database.CreateTableAsync<SelectedCharacter>();
        }

        // Add a single message to the database
        public async Task AddMessageAsync(Message message)
        {
            await _database.InsertAsync(message);
        }

        // Load all messages from the database
        public async Task<List<Message>> LoadAllMessagesAsync()
        {
            return await _database.Table<Message>().ToListAsync();
        }

        // Save the selected Character ID
        public async Task SaveSelectedCharacterAsync(int selectedCharacterId)
        {
            await _database.DeleteAllAsync<SelectedCharacter>(); // Clear previous selection
            await _database.InsertAsync(new SelectedCharacter { CharacterId = selectedCharacterId });
        }

        // Load the selected Character ID
        public async Task<int?> LoadSelectedCharacterIdAsync()
        {
            var selectedCharacter = await _database.Table<SelectedCharacter>().FirstOrDefaultAsync();
            return selectedCharacter?.CharacterId;
        }
    }

    // Model for persisting the selected Character
    public class SelectedCharacter
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CharacterId { get; set; }
    }
}