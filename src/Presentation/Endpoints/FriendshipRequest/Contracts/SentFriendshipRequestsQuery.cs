using System.ComponentModel;

namespace Presentation.Endpoints.FriendshipRequest.Contracts;

public class SentFriendshipRequestsQuery
{
    [DefaultValue(0)]
    public int Page { get; set; }
}