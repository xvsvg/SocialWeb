namespace DataAccess.Contracts;

public interface IUserIdentifierProvider
{
    Guid UserId { get; }
}