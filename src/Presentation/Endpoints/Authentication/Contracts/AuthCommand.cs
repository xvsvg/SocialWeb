namespace Presentation.Endpoints.Authentication.Contracts;

public class AuthCommand
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}