using System;
using Microsoft.Maui.Controls;

namespace ChatApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MenuList.ItemTapped += OnMenuTapped;
            ContentFrame.Content = new ChatPage(); // Default to ChatPage
        }

        private void OnMenuTapped(object sender, ItemTappedEventArgs e)
        {
            string selectedMenu = e.Item as string;

            if (selectedMenu == "Create")
            {
                ContentFrame.Content = new CreationPage();
            }
            else
            {
                ContentFrame.Content = new ChatPage();
            }
        }
    }
}
