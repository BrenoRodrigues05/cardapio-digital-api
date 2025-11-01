using AutoMapper;
using cardapio_digital_api.DTOs;
using cardapio_digital_api.Models;

namespace cardapio_digital_api.Mappings
{
    /// <summary>
    /// Perfil de configuração do AutoMapper responsável por definir
    /// os mapeamentos entre as entidades de domínio e seus respectivos DTOs.
    /// </summary>
    /// <remarks>
    /// Este perfil é automaticamente registrado no AutoMapper durante a inicialização
    /// da aplicação, conforme definido em <c>Program.cs</c>.
    /// 
    /// O objetivo é facilitar a conversão entre entidades e Data Transfer Objects (DTOs),
    /// evitando código manual de atribuição de propriedades.
    /// </remarks>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="AutoMapperProfile"/>
        /// e define todos os mapeamentos utilizados pela aplicação.
        /// </summary>
        public AutoMapperProfile()
        {
            // Cliente
            CreateMap<Cliente, ClienteCreateDTO>().ReverseMap(); // Mapeamento bidirecional
            CreateMap<Cliente, ClienteReadDTO>();                // Mapeamento somente de saída (entidade → DTO)

            // Produto
            CreateMap<Produto, ProdutoCreateDTO>().ReverseMap();
            CreateMap<Produto, ProdutoReadDTO>();

            // ItemPedido
            CreateMap<ItemPedido, ItemPedidoCreateDTO>().ReverseMap();
            CreateMap<ItemPedido, ItemPedidoReadDTO>();

            // Pedido
            CreateMap<Pedido, PedidoCreateDTO>().ReverseMap();
            CreateMap<Pedido, PedidoReadDTO>();

            // Restaurante
            CreateMap<Restaurante, RestauranteCreateDTO>().ReverseMap();
            CreateMap<Restaurante, RestauranteReadDTO>();
        }
    }
}
