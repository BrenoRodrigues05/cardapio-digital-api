using System.Collections.Generic;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para leitura e exibição das informações de um restaurante no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado em operações de consulta (GET) e contém
    /// os principais dados do restaurante, incluindo seus produtos e pedidos.
    /// </remarks>
    public class RestauranteReadDTO
    {
        /// <summary>
        /// Identificador único do restaurante.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nome do restaurante.
        /// </summary>
        /// <example>Restaurante Sabor Da Casa</example>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Endereço completo do restaurante.
        /// </summary>
        /// <example>Rua das Flores, 123 - São Paulo/SP</example>
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Categoria principal do restaurante.
        /// </summary>
        /// <example>Pizza</example>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefone do restaurante.
        /// </summary>
        /// <example>(11) 98765-4321</example>
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Horário de funcionamento do restaurante.
        /// </summary>
        /// <example>10:00 - 22:00</example>
        public string HorarioFuncionamento { get; set; } = string.Empty;

        /// <summary>
        /// Lista de produtos oferecidos pelo restaurante.
        /// </summary>
        /// <remarks>
        /// Cada produto inclui informações básicas como nome, descrição, preço e disponibilidade.
        /// </remarks>
        public List<ProdutoReadDTO> Produtos { get; set; } = new List<ProdutoReadDTO>();

        /// <summary>
        /// Lista de pedidos realizados neste restaurante.
        /// </summary>
        /// <remarks>
        /// Pode ser usada para relatórios e visualização de histórico de pedidos.
        /// </remarks>
        public List<PedidoReadDTO> Pedidos { get; set; } = new List<PedidoReadDTO>();
    }
}
