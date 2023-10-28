using Domain.Core.User;

namespace DataAccess.Contracts;

public interface IJwtProvider
{
    string Create(User user);
}