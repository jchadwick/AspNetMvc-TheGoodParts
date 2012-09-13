using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Website.Filters;

namespace Website.Models
{
    public class ListItemRequest
    {
        public long CategoryId { get; set; }
        public SelectList Categories { get; set; }

        /// <summary>
        /// Populated by the <see cref="CurrentUsernameValueProvider"/>
        /// </summary>
        public string CurrentUserName { get; set; }

        // Property with the same name as the Auction.SellerUsername
        // so that the Mapper will automatically map it by convention
        public string SellerUsername
        {
            get { return CurrentUserName; }
        }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 500, MinimumLength = 10)]
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Required]
        [Display(Name = "Starting Price")]
        [DataType(DataType.Currency)]
        [Range(1, double.MaxValue)]
        public decimal StartingPrice { get; set; }

        [Required]
        public int Duration { get; set; }

        public DateTime StartTime
        {
            get { return DateTime.Now; }
        }

        public DateTime EndTime
        {
            get { return StartTime.AddDays(Duration); }
        }

        public ListItemRequest()
        {
            Duration = 5;
            StartingPrice = 1;
        }
    }
}