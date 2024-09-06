using System;

namespace ChatApp.Models
{
    public class User
    {
        public string Id { get; set; } // Unique ID of the user
        public string CharacterName { get; set; } // Name of the user
        public string AvatarImage { get; set; } // URL or path to the user's avatar image

        // Additional description details about the user
        public Description Description { get; set; }

        // Any other existing fields used for updating the UI...
        public string Email { get; set; } // Example existing field

        public Color Color { get; set; } = Color.FromArgb("#A084F7");

        public override bool Equals(object obj)
        {
            if (obj is User otherUser)
            {
                return this.Id == otherUser.Id;
            }
            return false;
        }

        // Override GetHashCode to use the Id for generating hash code
        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }

        // Static method to create a User from a Character object
        public static User FromCharacter(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            // Map Character to User
            return new User
            {
                Id = character.Id,
                CharacterName = character.CharacterName,
                AvatarImage = character.AvatarImage,
                Description = new Description
                {
                    Gender = character.Description?.Gender,
                    Pronouns = character.Description?.Pronouns,
                    StageOfLife = character.Description?.StageOfLife,
                    Personality = character.Description?.Personality,
                    Details = character.Description?.Details,
                    GreetingMessage = character.Description?.GreetingMessage
                }
                // Email and AvatarImage are not mapped from Character
                // Set other fields in User as needed (like AvatarImage, Email, etc.)
            };
        }

    }

    // Class for detailed descriptions of the user
    public class Description
    {
        public string Gender { get; set; }
        public string Pronouns { get; set; }
        public string StageOfLife { get; set; }
        public string Personality { get; set; }
        public string Details { get; set; }
        public string GreetingMessage { get; set; }
    }
}