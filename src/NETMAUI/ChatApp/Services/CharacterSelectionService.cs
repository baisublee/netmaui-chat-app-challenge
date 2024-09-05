using ChatApp.ViewModels;
using ChatApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class CharacterSelectionService
    {
        private static CharacterSelectionService _instance;
        public static CharacterSelectionService Instance => _instance ??= new CharacterSelectionService();

        public CharacterViewModel SelectedCharacterViewModel { get; private set; }

        // Set the selected character
        public async void SetSelectedCharacter(CharacterViewModel character)
        {
            SelectedCharacterViewModel = character;

            // Persist the selected character ID
            await ChatPersistService.Instance.SaveSelectedCharacterId(character.Id);
        }

        // Load the selected character from persisted ID
        public async Task<CharacterViewModel> LoadSelectedCharacterAsync(List<Character> characters)
        {
            var characterId = await ChatPersistService.Instance.GetPersistedSelectedCharacterId();

            if (!string.IsNullOrEmpty(characterId))
            {
                // Find the matching Character from the provided list
                var selectedCharacter = characters.FirstOrDefault(c => c.Id == characterId);
                if (selectedCharacter != null)
                {
                    // Convert the Character to CharacterViewModel
                    var selectedCharacterViewModel = new CharacterViewModel
                    {
                        Id = selectedCharacter.Id,
                        Name = selectedCharacter.CharacterName,
                        Image = "rachel_green.png",
                    };

                    SetSelectedCharacter(selectedCharacterViewModel);
                    return selectedCharacterViewModel;
                }
            }
            return null;
        }
    }
}