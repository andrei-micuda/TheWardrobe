using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheWardrobe.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "The rating should have a value between 0 - 5!")]
        public int Rating { get; set; } // 1, .., 5

        public string RatingString
        {
            get
            {
                if (Rating > 4)
                    return "Excellent";
                else if (Rating > 3)
                    return "Above Average";
                else if (Rating > 2)
                    return "Average";
                else if (Rating > 1)
                    return "Below Average";
                else
                    return "Poor";
            }
        }

        [Required(ErrorMessage = "The review title is required!")]
        [StringLength(100, ErrorMessage = "The review title is too long!")]
        public string ReviewTitle { get; set; }

        public string ReviewBody { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}