using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Representa os dados necessários para criar uma nova avaliação.
    /// </summary>
    public class AvaliacaoCreateDTO
    {
        /// <summary>
        /// Nota atribuída pelo cliente (1 a 5).
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Nota { get; set; }

        /// <summary>
        /// Comentário opcional da avaliação.
        /// </summary>
        [MaxLength(500)]
        public string Comentario { get; set; } = string.Empty;

        /// <summary>
        /// Identificador do cliente autor da avaliação.
        /// </summary>
        [Required]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador do restaurante avaliado.
        /// </summary>
        [Required]
        public int RestauranteId { get; set; }
    }
}
