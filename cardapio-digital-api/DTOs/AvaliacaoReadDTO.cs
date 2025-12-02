using cardapio_digital_api.Models.Enums;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Representa os dados retornados pela API sobre uma avaliação.
    /// </summary>
    public class AvaliacaoReadDTO
    {
        /// <summary>
        /// Identificador da avaliação.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nota atribuída (1 a 5).
        /// </summary>
        public NotaEnum Nota { get; set; }

        /// <summary>
        /// Comentário opcional do cliente.
        /// </summary>
        public string Comentario { get; set; } = string.Empty;

        /// <summary>
        /// Data da avaliação.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Identificador do cliente autor da avaliação.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador do restaurante avaliado.
        /// </summary>
        public int RestauranteId { get; set; }
    }
}
