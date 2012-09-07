using System.Data.Entity;

namespace Website.Models
{
    public class AuctionContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AuctionContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuctionContext>());
        }
    }
}