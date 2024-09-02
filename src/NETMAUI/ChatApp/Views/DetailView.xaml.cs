using ChatApp.Models;
using ChatApp.Services;
using System.Text;
using System.Diagnostics;
using ChatApp.ViewModels; // Assuming CharacterViewModel is in this namespace

namespace ChatApp.Views
{
    public partial class DetailView : ContentPage
    {

        enum CommunicationOption
        {
            Chatbot,
            StreamChatbot,

            OllamaChatbot,

            EdgeAIChatbot,

            CAAService,
        }

        private static CommunicationOption streamingOption = CommunicationOption.CAAService;

        public DetailView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Access the passed CharacterViewModel if needed
            var character = BindingContext as CharacterViewModel;
            if (character != null)
            {
                Console.WriteLine($"DetailView for {character.Name}");
                // Update UI or perform actions based on the character
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Access the passed CharacterViewModel if needed
            var character = BindingContext as CharacterViewModel;
            if (character != null)
            {
                Console.WriteLine($"DetailView for {character.Name}");
                // Update UI or perform actions based on the character
            }
        }

        private async void MessageEntry_Completed(object sender, EventArgs e)
        {
            // Call the method directly or adapt if it requires parameters
            await HandleSendButtonClickedAsync();
            MessageEntry.Focus();
        }

        private async void ImgSendBtn_Clicked(object sender, EventArgs e)
        {
            await HandleSendButtonClickedAsync();
        }

        private async Task HandleSendButtonClickedAsync()
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

            string response = string.Empty;
            switch (streamingOption)
            {
                case CommunicationOption.Chatbot:
                    response = await GetChatbotResponseAsync(message);
                    Console.WriteLine($"Chatbot response: {response}");
                    var resp = new Message
                    {
                        Sender = MessageService.Instance.user1,
                        Time = DateTime.Now.ToString("HH:mm"),
                        Text = response,
                    };
                    await AddMessage(resp);
                    break;
                case CommunicationOption.StreamChatbot:
                    response = await GetStreamChatbotResponseAsync(message);
                    Console.WriteLine($"Chatbot response: {response}");
                    break;
                case CommunicationOption.OllamaChatbot:
                    response = await GetOllamaChatBotResponseAsync(message);
                    Console.WriteLine($"Chatbot response: {response}");
                    break;
                case CommunicationOption.EdgeAIChatbot:
                    response = await GetEdgeAIChatBotResponseAsync(message);
                    Console.WriteLine($"Chatbot response: {response}");
                    break;
                case CommunicationOption.CAAService:
                    response = await GetChatbotResponseFromCAAAsync(message);
                    Console.WriteLine($"Chatbot response: {response}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        private async Task<string> GetOllamaChatBotResponseAsync(string userInput)
        {
            StringBuilder completeResponse = new StringBuilder();

            await OllamaChatBotService.Instance.SendMessageAsync(userInput, async chunk =>
            {
                Console.Write(chunk);
                Debug.WriteLine(chunk + DateTime.Now.ToString("HH:mm:ss:fff"));
                completeResponse.Append(chunk);

                var resp = new Message
                {
                    Sender = MessageService.Instance.user1,
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = chunk,
                };

                await UpdateMessage(resp);

            });

            return completeResponse.ToString();
        }

        private async Task<string> GetEdgeAIChatBotResponseAsync(string userInput)
        {
            StringBuilder completeResponse = new StringBuilder();

            await EdgeAIService.Instance.StartChatAsync("2", userInput, async chunk =>
            {
                Console.Write(chunk);
                Debug.WriteLine(chunk + DateTime.Now.ToString("HH:mm:ss:fff"));
                completeResponse.Append(chunk);

                var resp = new Message
                {
                    Sender = MessageService.Instance.user1,
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = chunk,
                };

                await UpdateMessage(resp);

            });

            return completeResponse.ToString();
        }

        private async Task<string> GetChatbotResponseFromCAAAsync(string userInput)
        {
            StringBuilder completeResponse = new StringBuilder();

            await CAAService.Instance.StartChatAsync("2", userInput, async chunk =>
            {
                Console.Write(chunk);
                Debug.WriteLine(chunk + DateTime.Now.ToString("HH:mm:ss:fff"));
                completeResponse.Append(chunk);

                var resp = new Message
                {
                    Sender = MessageService.Instance.user1,
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = chunk,
                };

                await UpdateMessage(resp);

            });

            return completeResponse.ToString();
        }

        private async Task UpdateMessage(Message msg)
        {

            try
            {

                var lastItem = MessageService.Instance.User1MessageList.LastOrDefault();

                if (lastItem != null && lastItem.Sender != null)
                {
                    lastItem.Text += msg.Text;

                    MessagesCollectionView.ItemsSource = null;
                    MessagesCollectionView.ItemsSource = MessageService.Instance.User1MessageList;

                    MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: false);

                }
                else
                {
                    await AddMessage(msg);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

        }

        private async Task AddMessage(Message msg)
        {

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

        }

        private void ChangeImage(string imageName)
        {
            ProfileImage.Source = ImageSource.FromUri(new Uri(imageName));
        }

    }
}