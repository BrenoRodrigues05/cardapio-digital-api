using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para a leitura e exibição de informações de um pedido no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado em operações de consulta (GET) e contém
    /// os dados principais do pedido, incluindo informações do cliente,
    /// restaurante, itens do pedido e valor total calculado.
    /// </remarks>
    public class PedidoReadDTO
    {
        /// <summary>
        /// Identificador único do pedido.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Data e hora em que o pedido foi realizado.
        /// </summary>
        /// <example>2025-10-30T14:45:00Z</example>
        public DateTime DataPedido { get; set; }

        /// <summary>
        /// Status atual do pedido.
        /// </summary>
        /// <remarks>
        /// Os status possíveis são: <b>Aberto</b>, <b>Em Preparo</b>, <b>Entregue</b> e <b>Cancelado</b>.
        /// </remarks>
        /// <example>Em Preparo</example>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Identificador do cliente que realizou o pedido.
        /// </summary>
        /// <example>3</example>
        public int ClienteId { get; set; }

        /// <summary>
        /// Dados básicos do cliente associado ao pedido.
        /// </summary>
        /// <remarks>
        /// Inclui nome, e-mail e telefone do cliente.
        /// </remarks>
        public ClienteReadDTO? Cliente { get; set; }

        /// <summary>
        /// Identificador do restaurante onde o pedido foi realizado.
        /// </summary>
        /// <example>2</example>
        public int RestauranteId { get; set; }

        /// <summary>
        /// Dados básicos do restaurante responsável pelo pedido.
        /// </summary>
        /// <remarks>
        /// Inclui nome, categoria e telefone de contato.
        /// </remarks>
        public RestauranteReadDTO? Restaurante { get; set; }

        /// <summary>
        /// Lista de itens que compõem o pedido.
        /// </summary>
        /// <remarks>
        /// Cada item contém informações sobre o produto, quantidade e subtotal.
        /// </remarks>
        public List<ItemPedidoReadDTO> Itens { get; set; } = new();

        /// <summary>
        /// Valor total do pedido (soma de todos os subtotais dos itens).
        /// </summary>
        /// <example>87.90</example>
        public decimal ValorTotal { get; set; }
    }
}
