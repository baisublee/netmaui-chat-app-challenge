public interface IMainPageActions
{
    Task ShowAlert(string title, string message, string cancel);
    Task AddNewCharacter(string characterName);
}