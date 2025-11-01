using Microsoft.Extensions.Logging;

namespace cardapio_digital_api.Logging
{
    /// <summary>
    /// Representa as configurações utilizadas pelo provedor de logger personalizado
    /// <see cref="CustomLoggerProvider"/>.
    /// </summary>
    /// <remarks>
    /// Esta classe define o nível mínimo de log e o identificador de evento padrão
    /// utilizados pelas instâncias de <see cref="CustomerLogger"/>.
    /// </remarks>
    public class CustomLoggerProviderConfiguration
    {
        /// <summary>
        /// Obtém ou define o nível mínimo de log a ser registrado.
        /// </summary>
        /// <value>
        /// O valor padrão é <see cref="LogLevel.Warning"/>, o que significa que apenas
        /// mensagens de aviso, erro e críticas serão gravadas.
        /// </value>
        public LogLevel Loglevel { get; set; } = LogLevel.Warning;

        /// <summary>
        /// Obtém ou define o identificador de evento padrão associado às mensagens de log.
        /// </summary>
        /// <value>
        /// O valor padrão é <c>0</c>, indicando que nenhum identificador específico foi definido.
        /// </value>
        public int EventId { get; set; } = 0;
    }
}
