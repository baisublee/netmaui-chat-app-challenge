public interface IMainPageActions
{
    Task ShowAlert(string title, string message, string cancel);
    Task AddNewCharacter(object requestObject);
    Task<string> CreateImage();  // Method that returns a string (e.g., URL or path of the image)

}