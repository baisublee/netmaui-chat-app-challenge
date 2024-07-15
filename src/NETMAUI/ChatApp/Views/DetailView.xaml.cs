using System.Runtime.CompilerServices;
using ChatApp.Models;
using ChatApp.Services;
using System.Text;
using System.Diagnostics;

namespace ChatApp.Views
{
    public partial class DetailView : ContentPage
    {
        public DetailView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            // MessageEntry.Completed += MessageEntry_Completed;

        }

        private void MessageEntry_Completed(object sender, EventArgs e)
        {
            // Call the method directly or adapt if it requires parameters
            HandleSendButtonClicked();
            MessageEntry.Focus();
        }

        private async void ImgSendBtn_Clicked(object sender, EventArgs e)
        {
            await HandleSendButtonClicked();
        }

        private async Task HandleSendButtonClicked()
        {
            string message = MessageEntry.Text;

            MessageEntry.Text = "";

            var msg = new Message
            {
                Sender = null,
                Time = DateTime.Now.ToString("HH:mm"),
                Text = message,
            };

            await AddMessage(msg);

            // string response = await GetChatbotResponseAsync(message);
            // Console.WriteLine($"Chatbot response: {response}");

            bool IsStreaming = true;
            string response;

            if (IsStreaming)
            {
                response = await GetStreamChatbotResponseAsync(message);
                Console.WriteLine($"Chatbot response: {response}");

            }
            else
            {
                response = await GetChatbotResponseAsync(message);
                Console.WriteLine($"Chatbot response: {response}");

                var resp = new Message
                {
                    Sender = MessageService.Instance.user1,
                    // Time = "12:11",
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = response,
                };

                await AddMessage(resp);

            }


            var imageUrl = await ImageSearchService.Instance.SearchImageAsync(response);
            Console.WriteLine("Best matching image URL: " + imageUrl);
            if (
                imageUrl != null &&
                imageUrl != "null" &&
                imageUrl != ""
            )
            {
                ChangeImage(imageUrl);
            }
            if (VoiceCheckBox.IsChecked == true)
            {
                string text = response;
                string voiceresponse = await VoiceService.Instance.GenerateVoiceAsync(text);
                Console.WriteLine($"Response from server: {voiceresponse}");
            }

        }

        // This method simulates getting user text, calling the chatbot service, and returning the response.
        private async Task<string> GetChatbotResponseAsync(string userInput)
        {
            // Call the ChatBotService to generate a response
            string response = await ChatBotService.Instance.GenerateResponseAsync(userInput);
            return response;
        }

        private async Task<string> GetStreamChatbotResponseAsync(string userInput)
        {
            StringBuilder completeResponse = new StringBuilder();

            await StreamChatBotService.Instance.GenerateResponseAsync(userInput, 0.0, 1000, async chunk =>
            {
                Console.Write(chunk);
                Debug.WriteLine(chunk + DateTime.Now.ToString("HH:mm:ss:fff"));
                completeResponse.Append(chunk);
            
                var resp = new Message
                {
                    Sender = MessageService.Instance.user1,
                    // Time = "12:11",
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = chunk,
                };
            
                await UpdateMessage(resp);
            
            });

            return completeResponse.ToString();
        }

        private async Task UpdateMessage(Message msg)
        {

            // Device.BeginInvokeOnMainThread(async () =>
            // {

                try
                {
                    // await DisplayAlert("Alert", msg.Text, "OK");

                    var lastItem = MessageService.Instance.User1MessageList.LastOrDefault();

                    if (lastItem != null && lastItem.Sender != null)
                    {
                        lastItem.Text += msg.Text;

                        // MessageService.Instance.User1MessageList.Remove(lastItem);
                        
                        // await AddMessage(lastItem);
                        // OnPropertyChanged(nameof(lastItem));

                        // OnPropertyChanged(nameof(MessageService.Instance.User1MessageList));
                        // OnPropertyChanged(nameof(MessageService.Instance.User1MessageList));
                    } else {
                        await AddMessage(msg);
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                    Console.WriteLine($"Exception caught: {ex.Message}");
                }

            // });

            // Now that MessagesCollectionView is directly accessible, use it to scroll
            // await MessagesCollectionView.ScrollToAsync(msg, position: ScrollToPosition.End, animate: true);
        }



        private async Task AddMessage(Message msg)
        {

            // Device.BeginInvokeOnMainThread(() =>
            // {

                try
                {

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

            // });

            // Now that MessagesCollectionView is directly accessible, use it to scroll
            // await MessagesCollectionView.ScrollToAsync(msg, position: ScrollToPosition.End, animate: true);
        }

        private void ChangeImage(string imageName)
        {
            ProfileImage.Source = ImageSource.FromUri(new Uri(imageName));
        }

        // private void ChangeBackgroundImage(string imageName)
        // {
        //     RelatedImage.Source = ImageSource.FromUri(new Uri(imageName));
        // }


    }
}