namespace EventsAPI.Models
{
    public record class LoginModel(
        string Login, 
        string Password
    );

    public record RegisterModel(
        string Login,
        string Password,
        string Name,
        string LastName,
        DateTime BirthDateTime,
        string Email
    );
}
