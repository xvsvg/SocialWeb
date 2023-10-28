namespace Domain.Core.User;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);

    Task<User?> GetByNameAsync(string name);

    Task<IReadOnlySet<User>> GetAllAsync(int count, int page);

    Task<User> CreateAsync(User user);

    ValueTask<bool> CheckIfExistByName(string name);
}