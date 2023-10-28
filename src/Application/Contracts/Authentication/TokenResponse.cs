namespace Contracts.Authentication;

public sealed class TokenResponse
{
    public TokenResponse(string token)
    {
        Token = token;
    }

    public string Token { get; }
}