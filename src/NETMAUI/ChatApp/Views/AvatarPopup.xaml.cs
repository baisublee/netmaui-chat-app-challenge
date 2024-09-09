using CommunityToolkit.Maui.Views;
using System;

namespace ChatApp.Views
{
    public partial class AvatarPopup : Popup
    {
        public AvatarPopup()
        {
            InitializeComponent();
        }

        // Cancel button click handler - closes the popup without returning a value
        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(null);  // Close the popup without returning any data
        }

        // Add button click handler - retrieves the avatar description and closes the popup
        private void OnAddClicked(object sender, EventArgs e)
        {
            // Retrieve the text entered in the Editor (AvatarDescriptionEditor)
            string avatarDescription = AvatarDescriptionEditor.Text;

            // Close the popup and return the entered description as the result
            Close(avatarDescription);
        }

        // Create an image button click handler
        private void OnCreateImageClicked(object sender, EventArgs e)
        {
            // Here you would include the logic to create the avatar image, e.g., calling an API
            string avatarDescription = AvatarDescriptionEditor.Text;
            
            // Call your image generation logic (API or local process)
            Console.WriteLine($"Creating an image with description: {avatarDescription}");

            // Example: Return the description for the caller to use
            Close(avatarDescription);  // You could pass a different result if needed
        }
    }
}