namespace ChatApp.Views
{
    public partial class DetailView : ContentPage
    {
        public DetailView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ImgBackBtn_Clicked(object sender, EventArgs e)
        {
            // Handle the button click event here
            await DisplayAlert("", "Send button clicked", "Alright");
        }
    }
}