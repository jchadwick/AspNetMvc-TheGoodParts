namespace Common
{
    public class DataContext : System.Data.Entity.DbContext
    {
        public System.Data.Entity.DbSet<Auction> Auctions { get; set; }
        public System.Data.Entity.DbSet<Bid> Bids { get; set; }
        public System.Data.Entity.DbSet<Category> Categories { get; set; }
    }
}