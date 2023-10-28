using MediatR;

namespace Application.Core.Contracts;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}