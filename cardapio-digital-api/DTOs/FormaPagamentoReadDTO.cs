namespace cardapio_digital_api.DTOs
{
    public class FormaPagamentoReadDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Lista de pedidos associados.
        /// </summary>
        public List<int> PedidosIds { get; set; } = new();
    }
}
