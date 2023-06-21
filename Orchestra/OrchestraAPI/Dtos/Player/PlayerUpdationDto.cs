using OrchestraAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace OrchestraAPI.Dtos.Player
{
    public class PlayerUpdationDto
    {
        [Required(ErrorMessage = "The Player Name is required.")]
        public string? Name { get; set; }
    }
}
