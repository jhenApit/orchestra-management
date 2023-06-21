using System.ComponentModel.DataAnnotations;

namespace OrchestraAPI.Dtos.Player
{
    public class PlayerConcertUpdateDto
    {
        [Required(ErrorMessage = "The Concert is Required.")]
        public string? Concert { get; set; }
    }
}
