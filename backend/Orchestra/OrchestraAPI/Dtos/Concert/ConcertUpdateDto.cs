﻿using System.ComponentModel.DataAnnotations;
using OrchestraAPI.Models;

namespace OrchestraAPI.Dtos.Concert
{
    public class ConcertUpdateDto
    {
        [Required(ErrorMessage = "The Concert 'Name' is required")]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The Concert 'Description' is required")]
        [MaxLength(255, ErrorMessage = "Description cannot be longer than 255 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Concert 'Date' is required")]
        public DateTime PerformanceDate { get; set; }
        public string? Image { get; set; }

        public int OrchestraId { get; set; }
    }
}
