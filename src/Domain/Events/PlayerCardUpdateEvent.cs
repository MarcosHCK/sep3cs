using DataClash.Domain.Common;
using DataClash.Domain.Entities;

namespace DataClash.Domain.Events{
    public class PlayerCardUpdatedEvent : BaseEvent
        {
        public PlayerCard Item { get; }
    
        public PlayerCardUpdatedEvent (PlayerCard item)
            {
            Item = item;
            }
        }
}