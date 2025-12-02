namespace cardapio_digital_api.DTOs
{
    public class UsuarioReadDTO
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// CPF ou CNPJ do usuário.
        /// </summary>
        public string CpfCnpj { get; set; } = string.Empty;

        /// <summary>
        /// Endereço de email do usuário.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o usuário está ativo (opcional, se tiver campo no modelo).
        /// </summary>
        // public bool IsActive { get; set; } = true; // opcional
    }
}

