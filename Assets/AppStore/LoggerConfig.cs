using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;


namespace xPlugUniAdmissionManager;

public static class LoggerConfig
{
    public static IHostBuilder UseSerilogger(this WebApplicationBuilder builder)
    {
        var hostBuilder = builder.Host;

        var sp = builder.Services.BuildServiceProvider();

        var options = sp.GetService<IOptions<SerilogOptions>>();
        if (options == null || options.Value == null)
        {
            throw new ApplicationException("Invalid User Option Configurations");
        }

        var serilogOptions = options.Value;

        return hostBuilder.UseSerilog((context, configuration) =>
        {

            if (serilogOptions.EnableFile)
            {
                var filePath = serilogOptions.FileUrl;
                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentException("Logger file path has to be set.");

                configuration.WriteTo.File(filePath, rollingInterval: RollingInterval.Day);
            }

            if (serilogOptions.EnableConsole)
                configuration.WriteTo.Console();

            if (serilogOptions.EnableSeq)
            {
                var apiKey = serilogOptions.SeqApiKey;
                var seqUrl = serilogOptions.SeqURL;
                configuration.WriteTo.Seq(seqUrl, apiKey: apiKey);
            }

            configuration.Enrich.FromLogContext();

            var loggingLevelSwitch = new LoggingLevelSwitch { MinimumLevel = serilogOptions.LogEventLevel };
            configuration.MinimumLevel.ControlledBy(loggingLevelSwitch);
        });
    }
}
