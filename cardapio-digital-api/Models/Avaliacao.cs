using cardapio_digital_api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa uma avaliação realizada por um cliente para um restaurante.
    /// </summary>
    public class Avaliacao
    {
        /// <summary>
        /// Identificador único da avaliação.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nota atribuída pelo cliente ao restaurante.
        /// Valor deve estar entre 1 e 5.
        /// </summary>
        [Required(ErrorMessage = "A nota é obrigatória.")]
        public NotaEnum Nota { get; set; }

        /// <summary>
        /// Comentário opcional fornecido pelo cliente.
        /// Limite máximo de 500 caracteres.
        /// </summary>
        [MaxLength(500, ErrorMessage = "O comentário não pode exceder 500 caracteres.")]
        public string Comentario { get; set; } = string.Empty;

        /// <summary>
        /// Data em que a avaliação foi registrada.
        /// Definida automaticamente como UTC no momento da criação.
        /// </summary>
        public DateTime Data { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Identificador do cliente que realizou a avaliação.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Cliente que realizou a avaliação.
        /// </summary>
        public Cliente Cliente { get; set; } = null!;

        /// <summary>
        /// Identificador do restaurante que recebeu a avaliação.
        /// </summary>
        public int RestauranteId { get; set; }

        /// <summary>
        /// Restaurante avaliado pelo cliente.
        /// </summary>
        public Restaurante Restaurante { get; set; } = null!;

        /// <summary>
        /// Entregador avaliado pelo cliente.
        /// </summary>
        public Entregador Entregador { get; set; } = null!;

        /// <summary>
        /// Identificador do entregador que recebeu a avaliação.
        /// </summary>
        public int EntregadorId { get; set; }
    }
}
