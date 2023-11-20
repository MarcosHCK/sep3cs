using DataClash.Domain.Common;
using DataClash.Domain.Entities;

namespace DataClash.Domain.Events{
    public class PlayerCardDeletedEvent : BaseEvent
        {
        public PlayerCard Item { get; }
    
        public PlayerCardDeletedEvent (PlayerCard item)
            {
            Item = item;
            }
        }
}