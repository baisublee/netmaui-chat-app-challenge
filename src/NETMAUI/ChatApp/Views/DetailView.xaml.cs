using System.Runtime.CompilerServices;
using ChatApp.Models;
using ChatApp.Services;

namespace ChatApp.Views
{
    public partial class DetailView : ContentPage
    {
        public DetailView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ImgSendBtn_Clicked(object sender, EventArgs e)
        {

            string message = MessageEntry.Text;

            MessageEntry.Text = "";

            // Handle the button click event here
            // await DisplayAlert("", "Send button clicked " + message, "Alright");

            // Use the 'message' variable to process the entered text

            var msg = new Message
            {
                Sender = null,
                // Time = "12:11",
                Time = DateTime.Now.ToString("HH:mm"),
                Text = message,
            };

            //   MessageService.Instance.User1MessageList.Add(msg);

            //   MessageService.Instance.User1MessageList.Add(msg);

            await AddMessage(msg);


            string response = await GetChatbotResponseAsync(message);
            Console.WriteLine($"Chatbot response: {response}");

            var resp = new Message
            {
                Sender = MessageService.Instance.user1,
                // Time = "12:11",
                Time = DateTime.Now.ToString("HH:mm"),
                Text = response,
            };

            await AddMessage(resp);

            if(VoiceCheckBox.IsChecked == true) {
                string text = response;
                string voiceresponse = await VoiceService.Instance.GenerateVoiceAsync(text);
                Console.WriteLine($"Response from server: {voiceresponse}");
            }


            // Device.BeginInvokeOnMainThread(() =>
            // {
            //     MessageService.Instance.User1MessageList.Add(msg);
            // });

            // await ChatApp.Views.MessagesCollectionView.ScrollToAsync(msg, position: ScrollToPosition.End, animate: true);

            // ChatApp.Views.DetailView.
        }

        // This method simulates getting user text, calling the chatbot service, and returning the response.
        private async Task<string> GetChatbotResponseAsync(string userInput)
        {
            // Call the ChatBotService to generate a response
            string response = await ChatBotService.Instance.GenerateResponseAsync(userInput);
            return response;
        }




        private async Task AddMessage(Message msg)
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {

                try {

                    MessageService.Instance.User1MessageList.Add(msg);

                    var lastItem = MessageService.Instance.User1MessageList.LastOrDefault();
                    if (lastItem != null)
                    {
                        MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: true);
                    }
                  }
                catch (Exception ex)
                {
                    // Handle the exception here
                    Console.WriteLine($"Exception caught: {ex.Message}");
                }

            });

            // Now that MessagesCollectionView is directly accessible, use it to scroll
            // await MessagesCollectionView.ScrollToAsync(msg, position: ScrollToPosition.End, animate: true);


        }

        private void ChangeImage(string imageName)
        {
            ProfileImage.Source = imageName;
        }
    }
}