using System.Data.Entity;

namespace Data
{
  public class Connection : DbContext
    {
      public DbSet<Card> Card { get; set; }
    }

  public class Card
    {
      enum Quality
        {
          Common,
          Special,
          Epic,
          Legendary,
        }

      public int CardId { get; set; }
      public int CardCost { get; set; }
      public int InitialLevel { get; set; }
      public Quality Quality { get; set; }
      public string Description { get; set; }
      public string Name { get; set; }
    }
}
