﻿using ChatApp.Models;

namespace ChatApp.Views.Templates
{
    internal class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SenderMessageTemplate { get; set; }
        public DataTemplate ReceiverMessageTemplate { get; set; }

        public DataTemplate GreetingChatItemTemplate { get; set; }

        public DataTemplate OtherAppsItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = (Message)item;

            if(message.IsGreetingMessage) {
                return GreetingChatItemTemplate;
            }

            if(message.IsOtherAppPromotionMesaage) {
                return OtherAppsItemTemplate;
            }

            if (message.Sender == null)
                return ReceiverMessageTemplate;

            return SenderMessageTemplate;
        }
    }
}