namespace Domain.Core.Services;

public interface IPasswordHashChecker
{
    bool HashesMatch(string passwordHash, string password);
}