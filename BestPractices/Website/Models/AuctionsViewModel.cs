using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
    public class AuctionsViewModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalAuctionsCount { get; set; }
        public string SearchQuery { get; set; }

        public IEnumerable<AuctionViewModel> Auctions { get; set; }

        public int AuctionsCount
        {
            get { return (Auctions ?? Enumerable.Empty<AuctionViewModel>()).Count(); }
        }

    }
}