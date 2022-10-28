namespace NZWalks.API.Models.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Roles { get; set; }

    // Navigation properties:
    public List<User_Role> UserRoles { get; set; }
}
