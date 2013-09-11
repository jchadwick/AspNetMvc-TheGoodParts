using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
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
}