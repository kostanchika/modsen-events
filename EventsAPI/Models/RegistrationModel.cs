namespace EventsAPI.Models
{
    public record RegisterModel(
        string Login,
        string Password,
        string Name,
        string LastName,
        DateTime BirthDateTime,
        string Email
    );
}
