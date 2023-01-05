using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

public static class LogUtils{
    public static ILoggerFactory CreateLoggerFactory(ITestOutputHelper output)
	{
		return LoggerFactory.Create((lb) => {
			lb.AddXUnit(output);
			lb.SetMinimumLevel(LogLevel.Information);
            //lb.AddFilter((_, _, logLevel) => logLevel >= LogLevel.Information);
            lb.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        });
    }
    public static ILogger<T> CreateLogger<T>(ITestOutputHelper output)
    {
			
        return CreateLoggerFactory(output).CreateLogger<T>();
    }
}