using CommunityToolkit.Maui.Views;
using System;

namespace ChatApp.Views
{
    public partial class CreateAvatarPopup : Popup
    {
        public CreateAvatarPopup()
        {
            InitializeComponent();
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();  // Close the popup when "Cancel" is clicked
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            // Logic for the "Add" button (e.g., image creation)
            Close();  // Close the popup after handling the logic
        }
    }
}