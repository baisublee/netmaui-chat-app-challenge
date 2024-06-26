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
                Sender = MessageService.Instance.user1,
                // Time = "12:11",
                Time = DateTime.Now.ToString("HH:mm"),
                Text = message,
            };

            //   MessageService.Instance.User1MessageList.Add(msg);

            //   MessageService.Instance.User1MessageList.Add(msg);

            await AddMessage(msg);

            // Device.BeginInvokeOnMainThread(() =>
            // {
            //     MessageService.Instance.User1MessageList.Add(msg);
            // });

            // await ChatApp.Views.MessagesCollectionView.ScrollToAsync(msg, position: ScrollToPosition.End, animate: true);

            // ChatApp.Views.DetailView.
        }

        private async Task AddMessage(Message msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessageService.Instance.User1MessageList.Add(msg);

                var lastItem = MessageService.Instance.User1MessageList.LastOrDefault();
                if (lastItem != null)
                {
                    MessagesCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: true);
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