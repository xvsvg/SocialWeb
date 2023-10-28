using Application.Core.Contracts;
using Contracts.Authentication;
using DataAccess.Contracts;
using Domain.Common.Errors.User;
using Domain.Common.Utilities;
using Domain.Core.Services;
using Domain.Core.User;
using DomainUser = Domain.Core.User.User;

namespace Application.Handlers.Authentication.Commands.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenResponse>>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHashChecker _passwordHashChecker;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHashChecker passwordHashChecker,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHashChecker = passwordHashChecker;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByNameAsync(request.Name);

        if (user is null) 
            return DomainError.User.NotFound;

        var passwordValid = user.VerifyPasswordHash(request.Password, _passwordHashChecker);

        if (!passwordValid) 
            return Domain.Common.Errors.Identity.DomainError.Password.Incorrect;

        var token = _jwtProvider.Create(user);

        return new TokenResponse(token);
    }
}