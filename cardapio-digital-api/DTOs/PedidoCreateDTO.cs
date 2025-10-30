using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cardapio_digital_api.DTOs
{
    /// <summary>
    /// DTO utilizado para o cadastro de um novo pedido no sistema Cardápio Digital.
    /// </summary>
    /// <remarks>
    /// Este DTO é usado em requisições HTTP POST, representando as informações
    /// necessárias para registrar um pedido realizado por um cliente em um restaurante.
    /// </remarks>
    public class PedidoCreateDTO
    {
        /// <summary>
        /// Identificador do cliente que está realizando o pedido.
        /// </summary>
        /// <example>3</example>
        [Required(ErrorMessage = "O campo ClienteId é obrigatório.")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador do restaurante onde o pedido está sendo realizado.
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "O campo RestauranteId é obrigatório.")]
        public int RestauranteId { get; set; }

        /// <summary>
        /// Lista de itens incluídos no pedido.
        /// </summary>
        /// <remarks>
        /// Cada item deve conter o identificador do produto, a quantidade e o preço unitário.
        /// </remarks>
        [Required(ErrorMessage = "É necessário incluir ao menos um item no pedido.")]
        public List<ItemPedidoCreateDTO> Itens { get; set; } = new();

        /// <summary>
        /// Status inicial do pedido.
        /// </summary>
        /// <remarks>
        /// Valores possíveis: <b>Aberto</b>, <b>Em Preparo</b>, <b>Entregue</b> e <b>Cancelado</b>.
        /// Por padrão, o status é definido como <b>Aberto</b> ao criar um novo pedido.
        /// </remarks>
        /// <example>Aberto</example>
        public string Status { get; set; } = "Aberto";

        /// <summary>
        /// Data e hora da criação do pedido (definida automaticamente pelo sistema).
        /// </summary>
        /// <example>2025-10-30T14:45:00Z</example>
        public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    }
}
