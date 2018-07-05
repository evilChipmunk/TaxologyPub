using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Shared.Domain
{
    public interface IDomainEvent : INotification
    {
        DateTime DateTimeEventOccurred { get; }
    }

    public interface IDomainService
    {

    }




    public interface IDomainMediator
    {
        Task Publish<TNotification>(TNotification notification,
            CancellationToken cancellationToken = default(CancellationToken))
            where TNotification : IDomainNotification;
    }
    public interface IDomainNotification
    { }
    public interface IDomainNotificationHandler<in TNotification>
        where TNotification : IDomainNotification
    {
        Task Handle(TNotification notification,
            CancellationToken cancellationToken = default(CancellationToken));
    }

}