using DataClash.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DataClash.Application.PlayerCards.EventHandlers
{
    public class PlayerCardUpdatedEventHandler : INotificationHandler<PlayerCardUpdatedEvent>
    {
        private readonly ILogger<PlayerCardUpdatedEventHandler> _logger;

        public PlayerCardUpdatedEventHandler(ILogger<PlayerCardUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PlayerCardUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DataClash Domain Event: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}