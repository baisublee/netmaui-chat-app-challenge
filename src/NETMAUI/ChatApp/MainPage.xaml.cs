using System;
using Microsoft.Maui.Controls;
using ChatApp.Views; 

namespace ChatApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MenuList.ItemTapped += OnMenuTapped;
            ContentFrame.Content = new ContentView { Content = new DetailView().Content }; // Default to ChatPage
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
                ContentFrame.Content = new ContentView { Content = new DetailView().Content };
            }
        }
    }
}
