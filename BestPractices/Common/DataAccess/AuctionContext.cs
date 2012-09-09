using System.Data.Entity;

namespace Common.DataAccess
{
    public class AuctionContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}