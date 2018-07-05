using MassTransit;

namespace Taxology.WebAPI
{
    public interface IRequestClientFactory
    {
        IRequestClient<TRequest, TResult> Create<TRequest, TResult>() where TRequest : class where TResult : class;
    }
}