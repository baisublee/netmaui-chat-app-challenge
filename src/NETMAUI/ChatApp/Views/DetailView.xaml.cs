using ChatApp.Models;
using ChatApp.Services;
using System.Text;
using System.Diagnostics;
using ChatApp.ViewModels; // Assuming CharacterViewModel is in this namespace
using System.Collections.ObjectModel;

namespace ChatApp.Views
{
    public partial class DetailView : ContentPage
    {
        // public ObservableCollection<Message> Messages { get; set; }
        private DetailViewModel _viewModel;

        enum CommunicationOption
        {
            Chatbot,
            StreamChatbot,

            OllamaChatbot,

            EdgeAIChatbot,

            CAAService,
        }

        private static CommunicationOption streamingOption = CommunicationOption.CAAService;

        private CharacterViewModel _character;

        // public CharacterViewModel Character
        // {
        //     get { return _character; }
        //     set
        //     {
        //         _character = value;
        //         _user = new User
        //         {
        //             Image = value.Image,
        //             Name = value.Name,
        //             Color = Color.FromArgb("#A084F7"),
        //         };
        //     }
        // }

        private User _user;

        public DetailView()
        {
            InitializeComponent();

            // Let's get selected character from the CharacterSelectionService
            var selectedCharacter = CharacterSelectionService.Instance.SelectedCharacterViewModel;

            if (selectedCharacter != null)
            {
                _character = selectedCharacter;
                _user = new User
                {
                    Id = selectedCharacter.Id,
                    AvatarImage = selectedCharacter.Image,
                    CharacterName = selectedCharacter.Name,
                };
                _viewModel = new DetailViewModel();
                _viewModel.LoadMessagesForUser(_user);



                // MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: false);

                // Defer the scrolling operation to ensure the layout is complete
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagesCollectionView.ItemsSource = null;
                    MessagesCollectionView.ItemsSource = MessageService.Instance.GetMessagesForUser(_user);
                    var lastItem = MessageService.Instance.GetMessagesForUser(_user).LastOrDefault();
                    if (lastItem != null)
                    {
                        MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: false);
                    }
                });

            }

            // Initialize the ViewModel from BindingContext


            // NavigationPage.SetHasNavigationBar(this, false);
            // Initialize the ViewModel from BindingContext
            // viewModel = this.BindingContext as DetailViewModel;
            // System.Diagnostics.Debug.WriteLine("BindingContext: " + BindingContext);


        }

        // Override OnAppearing to refresh data when the view appears
        // protected override void OnAppearing()
        // {
        //     base.OnAppearing();

        //     // Ensure ViewModel is still properly set
        //     if (_viewModel != null)
        //     {
        //         Debug.WriteLine("DetailView appearing, refreshing data.");
        //         _viewModel.LoadMessagesForUser(_user);

        //         // Optionally refresh CollectionView data
        //         //MessagesCollectionView.ItemsSource = viewModel.Messages;
        //     }
        // }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Access the passed CharacterViewModel if needed
            if (_viewModel != null && _viewModel.User != null)
            {
                Debug.WriteLine("DetailView appearing, refreshing data.");
                _viewModel.LoadMessagesForUser(_viewModel.User);
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

            var msg = new Message()
            {
                CharacterId = _character.Id,
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
                        CharacterId = _character.Id,
                        Sender = _user,
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
                    CharacterId = _character.Id,
                    Sender = _user,
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
                    CharacterId = _character.Id,
                    Sender = _user,
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

            await EdgeAIService.Instance.StartChatAsync(_character.Id, userInput, async chunk =>
            {
                Console.Write(chunk);
                Debug.WriteLine(chunk + DateTime.Now.ToString("HH:mm:ss:fff"));
                completeResponse.Append(chunk);

                var resp = new Message
                {
                    CharacterId = _character.Id,
                    Sender = _user,
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

            await CAAService.Instance.StartChatAsync(_character.Id, userInput, async (chunk, done) =>
            {

                Debug.WriteLine($"{done} {chunk} {DateTime.Now.ToString("HH:mm:ss:fff")}");
                completeResponse.Append(chunk);

                var resp = new Message
                {
                    CharacterId = _character.Id,
                    Sender = _user,
                    Time = DateTime.Now.ToString("HH:mm"),
                    Text = chunk,
                };

                await UpdateMessage(resp);

                if (done)
                {
                    var lastItem = MessageService.Instance.GetMessagesForUser(_user).LastOrDefault();

                    if (lastItem != null && lastItem.Sender != null)
                    {
                        Debug.WriteLine("Since message is completed, will persist it");
                        await ChatPersistService.Instance.SaveMessageAsync(lastItem);
                    }
                }

            });



            return completeResponse.ToString();
        }

        private async Task UpdateMessage(Message msg)
        {

            try
            {

                var lastItem = MessageService.Instance.GetMessagesForUser(_user).LastOrDefault();

                if (lastItem != null && lastItem.Sender != null)
                {
                    Debug.WriteLine("Updating last message" + msg.Text);
                    lastItem.Text += msg.Text;

                    MessagesCollectionView.ItemsSource = null;
                    MessagesCollectionView.ItemsSource = MessageService.Instance.GetMessagesForUser(_user);

                    MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: false);

                }
                else
                {
                    await AddMessage(msg, false);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

        }

        private async Task AddMessage(Message msg, bool persist = true)
        {

            try
            {
                await MessageService.Instance.AddMessageToCharacterId(_character.Id, msg, persist);

                var lastItem = MessageService.Instance.GetMessagesForUser(_user).LastOrDefault();
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