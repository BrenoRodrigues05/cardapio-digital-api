using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para criação de novos restaurantes no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Contém apenas os campos necessários para o cadastro inicial de um restaurante.
    /// </remarks>
    public class RestauranteCreateDTO
    {
        /// <summary>
        /// Nome do restaurante.
        /// </summary>
        /// <example>Restaurante Sabor Da Casa</example>
        [Required(ErrorMessage = "O nome do restaurante é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Endereço completo do restaurante.
        /// </summary>
        /// <example>Rua das Flores, 123 - São Paulo/SP</example>
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Categoria principal do restaurante (ex.: Pizza, Lanches, Japonês).
        /// </summary>
        /// <example>Pizza</example>
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefone para contato do restaurante.
        /// </summary>
        /// <example>(11) 98765-4321</example>
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O telefone informado não é válido.")]
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Horário de funcionamento do restaurante.
        /// </summary>
        /// <example>10:00 - 22:00</example>
        [Required(ErrorMessage = "O horário de funcionamento é obrigatório.")]
        public string HorarioFuncionamento { get; set; } = string.Empty;
    }
}
