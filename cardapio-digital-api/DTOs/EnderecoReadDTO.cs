namespace cardapio_digital_api.DTOs
{
    public class EnderecoReadDTO
    {
        public int Id { get; set; }
        public string Rua { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Complemento { get; set; } = string.Empty;
        public string PontoReferencia { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }
}
