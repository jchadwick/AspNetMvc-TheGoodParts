using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class DataContext : System.Data.Entity.DbContext
    {
        public System.Data.Entity.DbSet<Auction> Auctions { get; set; }
        public System.Data.Entity.DbSet<Bid> Bids { get; set; }
        public System.Data.Entity.DbSet<Category> Categories { get; set; }
    }


    public class Auction
    {
        [Key]
        public long Id { get; private set; }

        [Required]
        public virtual long CategoryId
        {
            get
            {
                if (_categoryId == null && Category != null)
                    return Category.Id;

                return _categoryId.GetValueOrDefault();
            }
            set { _categoryId = value; }
        }
        private long? _categoryId;
        public virtual Category Category { get; set; }

        [Required]
        public virtual string SellerUsername { get; set; }

        public virtual string TopBidderUsername { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 500, MinimumLength = 10)]
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public ItemCondition Condition { get; set; }
        
        [Display(Name ="Image URL")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Display(Name ="Thumbnail URL")]
        [DataType(DataType.ImageUrl)]
        public string ThumbnailUrl { get; set; }

        [Display(Name ="Current Price")]
        [DataType(DataType.Currency)]
        public decimal? CurrentPrice { get; set; }

        [Required]
        [Display(Name ="Starting Price")]
        [DataType(DataType.Currency)]
        [Range(1, double.MaxValue)]
        public decimal StartingPrice { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; private set; }

        [Required]
        [Display(Name ="Start Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name ="End Time")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Featured Auction?")]
        public bool IsFeatured { get; set; }

        public virtual List<Bid> Bids { get; private set; }

        public Auction()
        {
            Created = DateTime.Now;
            Bids = new List<Bid>();
        }

        public void PlaceBid(string username, decimal amount)
        {
            if (CurrentPrice != null && amount <= CurrentPrice)
                throw new ApplicationException("Bid amount must exceed current price");

            TopBidderUsername = username;
            CurrentPrice = amount;
            Bids.Add(new Bid { AuctionId = Id, Username = username, BidAmount = amount });
        }
    }

    public class Bid
    {
        [Key]
        public long Id { get; private set; }

        [Required]
        public long AuctionId { get; set; }
        public virtual Auction Auction { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Bid Amount")]
        [DataType(DataType.Currency)]
        public decimal BidAmount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; private set; }

        public Bid()
        {
            Timestamp = DateTime.Now;
        }
    }

    public class Category
    {
        [Key]
        public long Id { get; private set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Key
        {
            get { return _key = (_key ?? Name.ToLower().Replace(" ", "_")); }
            set { _key = value; }
        }
        private string _key;

        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string Name { get; set; }
    }

    public enum ItemCondition
    {
        Used,
        New,
        Other,
    }
}