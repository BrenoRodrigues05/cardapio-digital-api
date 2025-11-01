using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace cardapio_digital_api.Logging
{
    /// <summary>
    /// Implementação personalizada de <see cref="ILogger"/> que grava logs em um arquivo de texto.
    /// </summary>
    public class CustomerLogger : ILogger
    {
        private readonly string name;
        private readonly CustomLoggerProviderConfiguration loggerProviderConfiguration;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CustomerLogger"/>.
        /// </summary>
        /// <param name="name">Nome da categoria ou contexto do logger.</param>
        /// <param name="loggerProviderConfiguration">Configuração personalizada do provedor de logger.</param>
        public CustomerLogger(string name, CustomLoggerProviderConfiguration loggerProviderConfiguration)
        {
            this.name = name;
            this.loggerProviderConfiguration = loggerProviderConfiguration;
        }

        /// <summary>
        /// Cria um escopo lógico para agrupar mensagens de log relacionadas.
        /// </summary>
        /// <typeparam name="TState">Tipo do estado associado ao escopo.</typeparam>
        /// <param name="state">Objeto que identifica o escopo do log.</param>
        /// <returns>
        /// Retorna <see langword="null"/> pois esta implementação não utiliza escopos.
        /// </returns>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        /// <summary>
        /// Verifica se o log está habilitado para o nível de log especificado.
        /// </summary>
        /// <param name="logLevel">Nível de log a ser verificado.</param>
        /// <returns>
        /// <see langword="true"/> se o nível de log atual for igual ao configurado;
        /// caso contrário, <see langword="false"/>.
        /// </returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerProviderConfiguration.Loglevel;
        }

        /// <summary>
        /// Registra uma mensagem de log no arquivo de texto.
        /// </summary>
        /// <typeparam name="TState">Tipo do estado associado à mensagem de log.</typeparam>
        /// <param name="logLevel">Nível de severidade da mensagem.</param>
        /// <param name="eventId">Identificador do evento relacionado ao log.</param>
        /// <param name="state">Informações de estado do log.</param>
        /// <param name="exception">Exceção associada, caso exista.</param>
        /// <param name="formatter">Função que formata o estado e a exceção em uma mensagem legível.</param>
        /// <remarks>
        /// O método grava a mensagem formatada no arquivo definido em <see cref="TextInFile(string)"/>.
        /// Cada entrada de log contém a data/hora, o nível do log, o ID do evento e a mensagem formatada.
        /// </remarks>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            string hour = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string message = $"{hour}/// {logLevel} : {eventId.Id} - {formatter(state, exception)}";

            TextInFile(message);
        }

        /// <summary>
        /// Escreve a mensagem de log no arquivo de texto definido.
        /// </summary>
        /// <param name="message">Mensagem a ser gravada no arquivo de log.</param>
        /// <remarks>
        /// O método abre o arquivo em modo de adição (<c>append</c> = true) e grava a mensagem em uma nova linha.
        /// Caso ocorra uma exceção durante a escrita, uma mensagem de erro é exibida no console.
        /// </remarks>
        private void TextInFile(string message)
        {
            string path = @"C:\Users\CSM\Desktop\CURSOS\ASP NET CORE\LOGGING\Cardapio-digital.txt";

            using (StreamWriter stream = new StreamWriter(path, true))
            {
                try
                {
                    stream.WriteLine(message);
                    stream.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Erro ao gravar log no arquivo: {message}");
                }
            }
        }
    }
}
