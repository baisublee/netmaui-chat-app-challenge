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
                Time= "12:11",
                Text= message, 
              };

            //   MessageService.Instance.User1MessageList.Add(msg);

                  MessageService.Instance.User1MessageList.Add(msg);


            //   Device.BeginInvokeOnMainThread(() => {
            //       MessageService.Instance.User1MessageList.Add(msg);
            //   });

            
        }
    }
}