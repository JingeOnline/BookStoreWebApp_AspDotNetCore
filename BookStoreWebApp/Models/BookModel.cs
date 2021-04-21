using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookStoreWebApp.Models
{
    public class BookModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="This field cannot be null.")]
        public string Title { get; set; }

        [MaxLength(30,ErrorMessage ="Author name cannot longer than 30 characters.")]
        public string Author { get; set; }

        public string Language { get; set; }
        public string Category { get; set; }

        [Display(Name ="Upload the cover image for your book")]
        public IFormFile CoverImage { get; set; }

        public string CoverImageUrl { get; set; }
    }
}
