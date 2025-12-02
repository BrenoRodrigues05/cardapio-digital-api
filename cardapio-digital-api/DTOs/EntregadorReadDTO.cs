namespace cardapio_digital_api.DTOs
{
    public class EntregadorReadDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// IDs dos pedidos entregues.
        /// </summary>
        public List<int> PedidosEntreguesIds { get; set; } = new();

        /// <summary>
        /// IDs das avaliações recebidas.
        /// </summary>
        public List<int> AvaliacoesIds { get; set; } = new();
    }
}
