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

        [Required]
        public string ReviewTitle { get; set; }

        public string ReviewBody { get; set; }
        public DateTime DateAdded { get; set; }
        public int? UserId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}