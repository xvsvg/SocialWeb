using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Mapping;
using Contracts.Common;
using Contracts.Images;
using DataAccess.Contracts;
using Domain.Common.Errors.Identity;
using Domain.Common.Utilities;
using Domain.Core.Friendship;
using Domain.Core.Image;
using Domain.Core.User;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Image.Queries.GetImagesForSpecificUser;

public sealed class GetImagesForSpecificUserQueryHandler
    : IQueryHandler<GetImagesForSpecificUserQuery, Result<PagedResponse<ImageResponse>>>
{
    private readonly PaginationConfiguration _configuration;
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUserRepository _userRepository;

    public GetImagesForSpecificUserQueryHandler(
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository,
        IOptions<PaginationConfiguration> configuration,
        IFriendshipRepository friendshipRepository)
    {
        _userIdentifierProvider = userIdentifierProvider;
        _userRepository = userRepository;
        _configuration = configuration.Value;
        _friendshipRepository = friendshipRepository;
    }

    public async Task<Result<PagedResponse<ImageResponse>>> Handle(GetImagesForSpecificUserQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _userIdentifierProvider.UserId;
        var currentUser = await _userRepository.GetByIdAsync(currentUserId);

        if (currentUser is null)
            return DomainError.GeneralError.Unauthorized;

        // image entity could've keep userId, thus db load would be less
        if (currentUserId.Equals(request.UserId))
        {
            var gallery = currentUser!.Gallery
                .Select(x => x.ToDto())
                .Skip((request.Page - 1) * _configuration.RecordsPerPage)
                .Take(_configuration.RecordsPerPage)
                .ToList();

            return new PagedResponse<ImageResponse>
            {
                Bunch = gallery,
                RecordPerPage = _configuration.RecordsPerPage,
                CurrentPage = request.Page,
                TotalPages = currentUser!.Gallery.Count / _configuration.RecordsPerPage
            };
        }

        var specifiedUser = await _userRepository.GetByIdAsync(request.UserId);

        if (specifiedUser is null) 
            return Domain.Common.Errors.User.DomainError.User.NotFoundFor(request.UserId);

        var friends = await _friendshipRepository.CheckIfFriendsAsync(currentUser, specifiedUser);
        if (friends is false) 
            return DomainError.GeneralError.InvalidPermissions;

        var specifiedUserGallery = specifiedUser.Gallery
            .Select(x => x.ToDto())
            .Skip((request.Page - 1) * _configuration.RecordsPerPage)
            .Take(_configuration.RecordsPerPage)
            .ToList();
        
        return new PagedResponse<ImageResponse>
        {
            Bunch = specifiedUserGallery,
            CurrentPage = request.Page,
            RecordPerPage = _configuration.RecordsPerPage,
            TotalPages = specifiedUser.Gallery.Count / _configuration.RecordsPerPage
        };
    }
}