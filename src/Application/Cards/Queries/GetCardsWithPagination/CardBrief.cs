
using DataClash.Domain.Enums;


namespace DataClash.Application.Cards.Queries.GetCard
{
    public class CardBrief
   {
     public long Id { get; init; }
     public string? Description { get; init; }
     public double ElixirCost { get; init; }
     public long InitialLevel { get; init; }
     public string? Name { get; init; }
     public Quality Quality { get; init; }
     public string? Picture { get; init; }
   }

}