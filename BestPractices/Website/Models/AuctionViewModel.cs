using System;
using Common;

namespace Website.Models
{
    public class AuctionViewModel
    {
        /***** "Copied" fields *****/

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ItemCondition Condition { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime Created { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsFeatured { get; set; }


        /***** "Flattened" fields *****/

        public long CategoryId { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryName { get; set; }

        public long SellerId { get; set; }
        public string SellerUsername { get; set; }
        public int SellerFeebackPercentage { get; set; }
        public string SellerFeebackPercentageDisplay
        {
            get { return SellerFeebackPercentage + "%"; }
        }

        public long? TopBidderId { get; set; }
        public string TopBidderUsername { get; set; }



        /***** "Smart" fields *****/

        public string CurrentPriceDisplay
        {
            get
            {
                decimal currentPrice = CurrentPrice ?? StartingPrice;
                return currentPrice.ToString("c");
            }
        }

        public string EndsIn
        {
            get
            {
                var remainingTime = RemainingTime;

                if (EndsToday)
                {
                    if (remainingTime > TimeSpan.FromHours(1))
                        return string.Format("{0} hours, {1} minutes", 
                                             remainingTime.Hours, remainingTime.Minutes);
                    
                    return string.Format("{0} minutes", remainingTime.Minutes);
                }

                return string.Format("{0} days, {1} hours", 
                                        remainingTime.Days, remainingTime.Hours);
            }
        }

        public bool EndsToday
        {
            get { return RemainingTime <= TimeSpan.FromDays(1); }
        }

        public bool HasBids
        {
            get { return CurrentPrice != null; }
        }

        public decimal MinimumBid
        {
            get
            {
                if(CurrentPrice == null)
                    return StartingPrice;

                return (CurrentPrice + .10m).Value;
            }
        }
        public string MinimumBidDisplay
        {
            get { return MinimumBid.ToString("g"); }
        }

        public TimeSpan RemainingTime
        {
            get { return EndTime - DateTime.Now; }
        }
    }
}