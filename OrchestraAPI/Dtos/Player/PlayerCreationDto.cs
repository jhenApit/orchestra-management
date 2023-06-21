using OrchestraAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace OrchestraAPI.Dtos.Player
{
    public class PlayerCreationDto
    {
        [Required(ErrorMessage = "The Player name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The UserId is Required.")]
        public int UserId { get; set; }
    }
}
