using DataClash.Domain.Common;
using DataClash.Domain.Entities;

namespace DataClash.Domain.Events
{
  public class PlayerCardCreatedEvent : BaseEvent
    {
      public PlayerCard Item { get; }

      public PlayerCardCreatedEvent (PlayerCard item)
        {
          Item = item;
        }
    }

}