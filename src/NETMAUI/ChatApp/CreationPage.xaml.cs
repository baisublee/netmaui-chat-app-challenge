using System;
using Microsoft.Maui.Controls;
using ChatApp.ViewModels;

namespace ChatApp
{
    public partial class CreationPage : ContentPage
    {
        public CreationPage()
        {
            InitializeComponent();
            BindingContext = new CreationPageViewModel();
        }
    }
}