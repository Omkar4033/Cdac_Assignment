using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class FoodItemDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Food name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Food name must be between 3 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z].*", ErrorMessage = "Food name must start with a letter.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, 10000000, ErrorMessage = "Price must be between 1 and 10,000,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

       
        [Range(1, 100000, ErrorMessage = "Calories must be between 1 and 100,000.")]
        public int? Calories { get; set; }

        
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Please specify if the item is vegetarian.")]
        public bool IsVegetarian { get; set; } = true;

        
        public DateTime CreatedAt { get; set; }

      
        [Url(ErrorMessage = "Invalid image URL format.")]
        public string ImageUrl { get; set; }
    }
}
