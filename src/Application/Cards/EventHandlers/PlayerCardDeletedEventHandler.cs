using DataClash.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DataClash.Application.PlayerCards.EventHandlers
{
    public class PlayerCardDeletedEventHandler : INotificationHandler<PlayerCardDeletedEvent>
    {
        private readonly ILogger<PlayerCardDeletedEventHandler> _logger;

        public PlayerCardDeletedEventHandler(ILogger<PlayerCardDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PlayerCardDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DataClash Domain Event: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}