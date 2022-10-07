using Serilog;

namespace UserApp.Api.Attributes
{
    //[AttributeUsage( AttributeTargets.Method )]
    public class InOutLoggingAttribute : DelegatingHandler
    {
        private readonly Serilog.ILogger _logger;

        public InOutLoggingAttribute(LoggerConfiguration configuration)
        {
            _logger = configuration.CreateLogger();
        }

        public InOutLoggingAttribute(Serilog.ILogger logger )
        {
            this._logger = logger;
        }

    }
}
