using System.ComponentModel.DataAnnotations;

namespace FarmersAppWithSearch.Models
{
    public class FarmerViewModel
    {
        public int Id { get; set; }

        [Required]

        public string FullName { get; set; } = string.Empty;

        [Required]

        public string FarmerCode { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? FarmName { get; set; }

        public string? Location { get; set; }

        // store blob image URL in the database

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }   
    }
}

