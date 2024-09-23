using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Plc.Command;

// Install packages:
// Microsoft.Extensions.Configuration.Json (read file)
// Microsoft.Extensions.Options.ConfigurationExtensions
// Microsoft.Extensions.Configuration
// Microsoft.Extensions.DependencyInjection
// Microsoft.Extensions.Options

namespace Plc.Test;

public class PlcOptionsTests
{
    private readonly string _fileConfig = "appsettings.json";
    private readonly string _sectionName = "PlcConfig";

    [Fact]
    public void PlcOptions_Should_Bind_From_AppSettings()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(_fileConfig)
            .Build();

        var services = new ServiceCollection();
        services.Configure<PlcOptions>(configuration.GetSection(_sectionName));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetService<IOptions<PlcOptions>>()?.Value;

        // Assert
        Assert.NotNull(options);
        Assert.Equal("S71200", options.CpuType);
        Assert.Equal(S7.Net.CpuType.S71200, options.GetCpuType());
        Assert.Equal("192.168.0.13", options.IpAddress);
        Assert.Equal(0, options.Rack);
        Assert.Equal(0, options.Slot);

        Assert.Equal("DB26.DBX18.0", options.Address.Correct);
        Assert.Equal("DB26.DBX18.1", options.Address.Incorrect);
        Assert.Equal("DB26.DBD14", options.Address.Barcode);

        Assert.Equal("DB26.DBX0.0", options.Address.In);
        Assert.Equal("DB26.DBX0.1", options.Address.Out);
        Assert.Equal("DB26.DBX0.2", options.Address.Transfer);

        Assert.Equal("DB26.DBW2", options.Address.FromX);
        Assert.Equal("DB26.DBW4", options.Address.FromY);
        Assert.Equal("DB26.DBW6", options.Address.FromZ);

        Assert.Equal("DB26.DBW8", options.Address.ToX);
        Assert.Equal("DB26.DBW10", options.Address.ToY);
        Assert.Equal("DB26.DBW12", options.Address.ToZ);

        Assert.Equal(TimeSpan.FromMinutes(5), options.TimeOut);
    }
}