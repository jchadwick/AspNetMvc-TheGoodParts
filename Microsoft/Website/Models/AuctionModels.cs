using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Website.Models
{
    public class AuctionContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        public AuctionContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuctionContext>());
        }
    }


    public class Auction
    {
        [Key]
        public long Id { get; private set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
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
        public decimal StartingPrice { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; private set; }

        [Required]
        [Display(Name ="Start Time")]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(DateValidator), "AfterNow")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name ="End Time")]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(DateValidator), "AfterNow")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Featured Auction?")]
        public bool IsFeatured { get; set; }

        public Auction()
        {
            Created = DateTime.Now;
        }
    }

    public class Bid
    {
        [Key]
        public long Id { get; private set; }

        [Required]
        public long AuctionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name ="Bid Amount")]
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

    public class DateValidator
    {
        public static ValidationResult AfterNow(DateTime value)
        {
            if (value.ToUniversalTime() >= DateTime.UtcNow)
                return ValidationResult.Success;

            return new ValidationResult("Value cannot be in the past");
        }
    }
}