using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Year")]
        public int? Year { get; set; }
        [Required]
        [DisplayName("ISBN")]
        public string Isbn { get; set; }
        [StringLength(250)]
        public string Summary { get; set; }
        public string Image { get; set; }
        [DisplayName("Price")]
        public decimal? Price { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}
