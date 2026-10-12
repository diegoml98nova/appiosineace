using Microsoft.Extensions.Logging;

namespace SineaceAPP.Tests;

/// <summary>Doble de prueba de ILogger: acumula los mensajes por nivel.</summary>
internal sealed class DobleLogger : ILogger
{
    public List<(LogLevel Nivel, string Mensaje)> Registros { get; } = new();

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instancia;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter) => Registros.Add((logLevel, formatter(state, exception)));

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instancia = new();
        public void Dispose() { }
    }
}
