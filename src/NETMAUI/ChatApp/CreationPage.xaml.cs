using System;
using Microsoft.Maui.Controls;
using ChatApp.ViewModels;

namespace ChatApp
{
    public partial class CreationPage : ContentPage
    {
        public CreationPageViewModel ViewModel { get; set; }
        public CreationPage()
        {
            InitializeComponent();
            ViewModel = new CreationPageViewModel();
            BindingContext = ViewModel;

        }
    }
}