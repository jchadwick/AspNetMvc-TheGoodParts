using System.Data.Entity;

namespace Common.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserProfile> Users { get; set; }
    }
}