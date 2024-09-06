using System;

namespace ChatApp.Models
{
    // Flattened class for storing Message in SQL without complex User object
    public class MessageFlat
    {
        public int Id { get; set; } // Primary key, auto-increment
        public string CharacterId { get; set; } // Associated Character's ID
        public int CreatedAt { get; set; } // Epoch time of message creation

        // Store Sender information for SQL persistence
        public string SenderId { get; set; } = null;
        public string SenderCharacterName { get; set; } = null;
        public string SenderAvatarImage { get; set; } = null;

        public string Text { get; set; } // Message text
        public string Time { get; set; } // Message time

        // New properties for identifying the type of message
        public bool IsGreetingMessage { get; set; } = false; // To mark if it's a greeting message
        public bool IsOtherAppPromotionMessage { get; set; } = false; // To mark if it's a promotion message

        // Convert from MessageFlat to Message (uses the SenderId to fetch the User object)
        public static Message ToMessage(MessageFlat flatMessage)
        {
            if (flatMessage == null)
                throw new ArgumentNullException(nameof(flatMessage));

            // Create the Message object
            var message = new Message
            {
                Id = flatMessage.Id,
                CharacterId = flatMessage.CharacterId,
                CreatedAt = flatMessage.CreatedAt,
                Text = flatMessage.Text,
                Time = flatMessage.Time,
                SenderId = flatMessage.SenderId,
                IsGreetingMessage = flatMessage.IsGreetingMessage,
                IsOtherAppPromotionMesaage = flatMessage.IsOtherAppPromotionMessage
            };

            // If SenderId is not null, create a User object from the flat message's sender details
            if (!string.IsNullOrEmpty(flatMessage.SenderId))
            {
                message.Sender = new User
                {
                    Id = flatMessage.SenderId,
                    CharacterName = flatMessage.SenderCharacterName,
                    AvatarImage = flatMessage.SenderAvatarImage
                };
            }
            else
            {
                // If SenderId is null, assign null to Sender
                message.Sender = null;
            }

            return message;
        }

        // Convert from Message to MessageFlat
        public static MessageFlat FromMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            // Flatten the Message object into MessageFlat
            return new MessageFlat
            {
                Id = message.Id,
                CharacterId = message.CharacterId,
                CreatedAt = message.CreatedAt,
                Text = message.Text,
                Time = message.Time,
                SenderId = message.Sender?.Id ?? null, // Set SenderId if Sender exists, otherwise null
                SenderCharacterName = message.Sender?.CharacterName ?? null, // Extract SenderCharacterName from User
                SenderAvatarImage = message.Sender?.AvatarImage ?? null, // Extract SenderAvatarImage from User
                IsGreetingMessage = message.IsGreetingMessage, // Map IsGreetingMessage
                IsOtherAppPromotionMessage = message.IsOtherAppPromotionMesaage // Map IsOtherAppPromotionMessage
            };
        }
    }
}