using DataClash.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DataClash.Application.PlayerCards.EventHandlers
{
  public class PlayerCardCreatedEventHandler : INotificationHandler<PlayerCardCreatedEvent>
    {
      private readonly ILogger<PlayerCardCreatedEventHandler> _logger;

      public PlayerCardCreatedEventHandler (ILogger<PlayerCardCreatedEventHandler> logger)
        {
          _logger = logger;
        }

      public Task Handle (PlayerCardCreatedEvent notification, CancellationToken cancellationToken)
        {
          _logger.LogInformation ("DataClash Domain Event: {DomainEvent}", notification.GetType ().Name);
          return Task.CompletedTask;
        }
    }
}
