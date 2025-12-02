using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.Models
{
    /// <summary>
    /// Representa o endereço associado a um cliente.
    /// </summary>
    public class Endereco
    {
        /// <summary>
        /// Identificador único do endereço.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da rua onde o cliente reside.
        /// </summary>
        [Required(ErrorMessage = "O campo Rua é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Rua deve ter no máximo 100 caracteres.")]
        public string Rua { get; set; } = string.Empty;

        /// <summary>
        /// Número da residência ou estabelecimento.
        /// </summary>
        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        public int Numero { get; set; }

        /// <summary>
        /// Informações adicionais sobre o endereço, como apartamento, bloco ou sala.
        /// </summary>
        [MaxLength(100, ErrorMessage = "O campo Complemento deve ter no máximo 100 caracteres.")]
        public string Complemento { get; set; } = string.Empty;

        /// <summary>
        /// Ponto de referência próximo ao endereço do cliente.
        /// </summary>
        [Required(ErrorMessage = "O campo Ponto de Referência é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O campo Ponto de Referência deve ter no máximo 200 caracteres.")]
        public string PontoReferencia { get; set; } = string.Empty;

        /// <summary>
        /// Bairro onde o cliente reside.
        /// </summary>
        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Bairro deve ter no máximo 100 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        /// <summary>
        /// Cidade onde o cliente reside.
        /// </summary>
        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Cidade deve ter no máximo 100 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado onde o cliente reside.
        /// </summary>
        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Estado deve ter no máximo 100 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// CEP correspondente ao endereço do cliente.
        /// </summary>
        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [MaxLength(20, ErrorMessage = "O campo CEP deve ter no máximo 20 caracteres.")]
        public string Cep { get; set; } = string.Empty;

        /// <summary>
        /// Identificador do cliente ao qual o endereço pertence.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Cliente associado ao endereço.
        /// </summary>
        public Cliente Cliente { get; set; } = null!;
    }
}
