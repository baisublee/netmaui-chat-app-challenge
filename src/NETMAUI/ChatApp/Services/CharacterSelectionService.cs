using ChatApp.ViewModels;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class CharacterSelectionService
    {
        private static CharacterSelectionService _instance;
        public static CharacterSelectionService Instance => _instance ??= new CharacterSelectionService();

        public CharacterViewModel SelectedCharacterViewModel { get; private set; }

        private CharacterSelectionService() { }

        // Method to update the selected character and persist it
        public async Task SetSelectedCharacterAsync(CharacterViewModel character)
        {
            SelectedCharacterViewModel = character;
            await ChatPersistService.Instance.SaveSelectedCharacterAsync(character.Id);
        }

        // Method to load the selected character from the database
        public async Task LoadSelectedCharacterAsync(List<CharacterViewModel> allCharacters)
        {
            var selectedCharacterId = await ChatPersistService.Instance.LoadSelectedCharacterIdAsync();
            if (selectedCharacterId.HasValue)
            {
                SelectedCharacterViewModel = allCharacters.Find(c => c.Id == selectedCharacterId.Value);
            }
        }

        public void ClearSelectedCharacter()
        {
            SelectedCharacterViewModel = null;
        }
    }
}