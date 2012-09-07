using System;

namespace Website.Models
{
    public class AuctionViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime Created { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsFeatured { get; set; }


        public long CategoryId { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryName { get; set; }

        public long? TopBidderId { get; set; }
        public string TopBidderUsername { get; set; }

        public string CurrentPriceDisplay
        {
            get
            {
                decimal currentPrice = CurrentPrice ?? StartingPrice;
                return currentPrice.ToString("c");
            }
        }

        public bool HasBids
        {
            get { return CurrentPrice != null; }
        }
    }
}