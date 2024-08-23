using Microsoft.Maui.Controls;

namespace ChatApp
{
    public partial class OnboardingSelectCharacter : ContentPage
    {
        public OnboardingSelectCharacter()
        {
            InitializeComponent();
        }

        private async void OnAssistantClicked(object sender, EventArgs e)
        {
            // Logic to select the assistant and navigate to the next page
            var button = sender as ImageButton;
            if (button != null)
            {
                string assistantName = ""; // You can set the assistant name or identifier here
                if (button.Source.ToString().Contains("chris_image"))
                {
                    assistantName = "Chris";
                }
                else if (button.Source.ToString().Contains("rachel_image"))
                {
                    assistantName = "Rachel";
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else if (button.Source.ToString().Contains("olivia_image"))
                {
                    assistantName = "Olivia";
                }

                // Navigate to the next page, passing the selected assistant name or ID
                // await Navigation.PushAsync(new NextPage(assistantName)); // Replace 'NextPage' with your actual next page
            } 
        }

        private async void OnCreateNewClicked(object sender, EventArgs e)
        {
            // Logic to navigate to the creation page for a new assistant
            // await Navigation.PushAsync(new CreateNewAssistantPage()); // Replace 'CreateNewAssistantPage' with your actual page
        }
    }
}

