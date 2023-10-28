namespace DataAccess.Contracts;

public interface IPasswordHasher
{
    string HashPassword(string password);
}