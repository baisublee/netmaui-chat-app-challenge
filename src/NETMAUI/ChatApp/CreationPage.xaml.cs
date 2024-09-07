using System;
using Microsoft.Maui.Controls;
using ChatApp.ViewModels;
using System.Diagnostics;

namespace ChatApp
{
    public partial class CreationPage : ContentPage
    {
        // public CreationPageViewModel ViewModel { get; set; }

        public CreationPage()
        {
            InitializeComponent();
            // ViewModel = new CreationPageViewModel();
            // BindingContext = ViewModel;

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Debugging output when the BindingContext changes
            Debug.WriteLine($"BindingContext changed for CreationPage. New BindingContext: {BindingContext?.GetType().Name}");
            if (BindingContext is CreationPageViewModel vm)
            {
                Debug.WriteLine("ViewModel is correctly bound");
                Debug.WriteLine($"CharacterName: {vm.CharacterName}");
            }
            else
            {
                Debug.WriteLine("BindingContext is not set correctly");
            }
        }
    }
}

