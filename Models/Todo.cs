using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleDotnetMvc.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Entry cannot be longer than 100 characters")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        #nullable enable
        public string? Description { get; set; }
        #nullable disable

        [Required]
        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Completed At")]
        public DateTime? CompletedAt { get; set; }
    }
}
