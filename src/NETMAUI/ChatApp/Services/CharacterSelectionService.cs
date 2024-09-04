using ChatApp.ViewModels;

namespace ChatApp.Services
{
    public class CharacterSelectionService
    {
        // Singleton instance
        private static CharacterSelectionService _instance;
        public static CharacterSelectionService Instance => _instance ??= new CharacterSelectionService();

        // Property to store the selected character globally
        public CharacterViewModel SelectedCharacterViewModel { get; private set; }

        // Private constructor for singleton pattern
        private CharacterSelectionService() { }

        // Method to update the selected character
        public void SetSelectedCharacter(CharacterViewModel character)
        {
            SelectedCharacterViewModel = character;
        }

        // Method to retrieve the selected character
        public CharacterViewModel GetSelectedCharacter()
        {
            return SelectedCharacterViewModel;
        }

        // Optional method to clear the selected character
        public void ClearSelectedCharacter()
        {
            SelectedCharacterViewModel = null;
        }
    }
}