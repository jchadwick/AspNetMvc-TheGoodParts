using System;
using System.ComponentModel.DataAnnotations;
using Common.Entities;

namespace Website.Models
{
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

        public virtual long? TopBidderId { get; set; }
        public virtual UserProfile TopBidder { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 500, MinimumLength = 10)]
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
}