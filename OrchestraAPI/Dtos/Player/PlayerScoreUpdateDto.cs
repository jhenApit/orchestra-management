using System.ComponentModel.DataAnnotations;

namespace OrchestraAPI.Dtos.Player
{
    public class PlayerScoreUpdateDto
    {
        [Required(ErrorMessage = "The Player Score is Required.")]
        public decimal Score { get; set; }
    }
}
