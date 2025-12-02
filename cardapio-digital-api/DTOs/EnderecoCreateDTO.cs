using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// Representa os dados necessários para criar um endereço.
    /// </summary>
    public class EnderecoCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Rua { get; set; } = string.Empty;

        [Required]
        public int Numero { get; set; }

        [MaxLength(100)]
        public string Complemento { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string PontoReferencia { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Cep { get; set; } = string.Empty;

        [Required]
        public int ClienteId { get; set; }
    }
}
