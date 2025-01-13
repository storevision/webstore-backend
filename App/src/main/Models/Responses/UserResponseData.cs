namespace Webshop.App.src.main.Models.Responses;

public class UserResponseData
{
    public int id { get; set; }
    
    public string email { get; set; }
    
    public string displayName { get; set; }
    
    
    public UserResponseData(User user)
    {
        id = user.CustomerID;
        email = user.Email;
        displayName = user.DisplayName;
    }
}