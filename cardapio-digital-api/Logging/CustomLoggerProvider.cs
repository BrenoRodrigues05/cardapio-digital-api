using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace cardapio_digital_api.Logging
{
    /// <summary>
    /// Provedor de log personalizado responsável por criar e gerenciar instâncias de <see cref="CustomerLogger"/>.
    /// </summary>
    /// <remarks>
    /// Esta classe implementa <see cref="ILoggerProvider"/> e atua como um ponto central para
    /// fornecer loggers configurados com <see cref="CustomLoggerProviderConfiguration"/>.
    /// </remarks>
    public class CustomLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Configurações utilizadas para inicializar os loggers criados por este provedor.
        /// </summary>
        private readonly CustomLoggerProviderConfiguration loggerProviderConfiguration;

        /// <summary>
        /// Dicionário thread-safe que armazena e reutiliza instâncias de <see cref="CustomerLogger"/>
        /// associadas a diferentes categorias.
        /// </summary>
        private readonly ConcurrentDictionary<string, CustomerLogger> loggers = new();

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CustomLoggerProvider"/>.
        /// </summary>
        /// <param name="loggerProviderConfiguration">Configuração de comportamento dos loggers criados.</param>
        public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerProviderConfiguration)
        {
            this.loggerProviderConfiguration = loggerProviderConfiguration;
        }

        /// <summary>
        /// Cria ou recupera uma instância de <see cref="CustomerLogger"/> associada à categoria especificada.
        /// </summary>
        /// <param name="categoryName">Nome da categoria de log (geralmente o nome da classe que está registrando o log).</param>
        /// <returns>
        /// Uma instância de <see cref="ILogger"/> que grava logs conforme a configuração definida.
        /// </returns>
        /// <remarks>
        /// O método utiliza um <see cref="ConcurrentDictionary{TKey, TValue}"/> para garantir
        /// que a mesma instância de logger seja reutilizada para cada categoria.
        /// </remarks>
        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerProviderConfiguration));
        }

        /// <summary>
        /// Libera os recursos utilizados pelo provedor de logger.
        /// </summary>
        /// <remarks>
        /// Como os loggers não mantêm recursos não gerenciados, esta implementação é vazia.
        /// Entretanto, o método é necessário para cumprir o contrato da interface <see cref="IDisposable"/>.
        /// </remarks>
        public void Dispose()
        {
            // Nenhum recurso para liberar.
        }
    }
}
