using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
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
}