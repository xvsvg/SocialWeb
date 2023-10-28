using Application.Core.Contracts;
using Contracts.Authentication;
using DataAccess.Contracts;
using Domain.Common.Errors.Identity;
using Domain.Common.Utilities;
using Domain.Core.User;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.User.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Result<TokenResponse>>
{
    private readonly IDatabaseContext _context;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IDatabaseContext context)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _context = context;
    }

    public async Task<Result<TokenResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var alreadyExist = await _userRepository.CheckIfExistByName(request.Name);

        if (alreadyExist) return DomainError.Login.DuplicateLogin;

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = DomainUser.Create(request.Name, passwordHash);

        await _userRepository.CreateAsync(user);
        await _context.SaveChangesAsync(cancellationToken);
        var token = _jwtProvider.Create(user);

        return new TokenResponse(token);
    }
}