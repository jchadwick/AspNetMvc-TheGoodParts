using System.Collections.Generic;
using System.Linq;

namespace Common.DataAccess
{
    public interface IAuctionRepository
    {
        void Add(Auction auction);
        IQueryable<string> Autocomplete(string query, long? category);
        IQueryable<Auction> GetFeaturedAuctions();
        Auction Find(long auctionId);
        IQueryable<Auction> FindByCategoryId(long categoryId, out Category category);
        IQueryable<Auction> FindByCategoryKey(string categoryKey, out Category category);
        void SaveChanges();
        IQueryable<Auction> Search(string query, long? categoryId, out Category category);
    }

    public class AuctionRepository : IAuctionRepository
    {
        private readonly DataContext _context;

        public AuctionRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Auction auction)
        {
            _context.Auctions.Add(auction);
        }

        public IQueryable<string> Autocomplete(string query, long? category)
        {
            IEnumerable<Auction> auctions = _context.Auctions;

            if (category != null)
                auctions = auctions.Where(x => x.CategoryId == category.Value);

            var lowerQuery = query.ToLower();
            
            var titles =
                auctions.Select(x => x.Title)
                        .Where(x => x.ToLower().Contains(lowerQuery));

            return titles.AsQueryable();
        }

        public IQueryable<Auction> GetFeaturedAuctions()
        {
            return _context.Auctions.Where(x => x.IsFeatured);
        }

        public Auction Find(long auctionId)
        {
            return _context.Auctions.Find(auctionId);
        }

        public IQueryable<Auction> FindByCategoryId(long categoryId, out Category category)
        {
            category = _context.Categories.Find(categoryId);

            if (category == null)
                return Enumerable.Empty<Auction>().AsQueryable();

            return category.Auctions.AsQueryable();
        }

        public IQueryable<Auction> FindByCategoryKey(string categoryKey, out Category category)
        {
            category = _context.Categories.FirstOrDefault(x => x.Key == categoryKey);

            if (category == null)
                return Enumerable.Empty<Auction>().AsQueryable();

            return category.Auctions.AsQueryable();
        }

        public IQueryable<Auction> Search(string query, long? categoryId, out Category category)
        {
            category = _context.Categories.Find(categoryId);


            IQueryable<Auction> auctions;

            if (categoryId == null || category == null)
            {
                auctions = _context.Auctions;
            }
            else
            {
                auctions = category.Auctions.AsQueryable();
            }


            if (!string.IsNullOrWhiteSpace(query))
            {
                auctions = auctions.Where(x =>
                    ((x.Title ?? "") + (x.Description ?? ""))
                        .ToLower()
                        .IndexOf(query.ToLower()) >= 0);
            }


            return auctions.OrderBy(x => x.EndTime);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
