namespace ClusterDashboard.Infrastructure.Services;

using Renci.SshNet;
using ClusterDashboard.Application.Interfaces;
using System.Text;
using System.Globalization;
using Microsoft.Extensions.Logging;

public class DeviceTemperatureService : IDeviceTemperatureService
{
	private readonly ILogger<DeviceTemperatureService> _logger;

	public DeviceTemperatureService(ILogger<DeviceTemperatureService> logger)
	{
		_logger = logger;
	}
	public Task<double?> GetTemperatureAsync(string hostname, string username, string privateKey)
	{
		return Task.Run(() =>
		{
			try
			{
				_logger.LogInformation($"Try host: {hostname} with user {username}");

				using var privateKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(privateKey));
				var keyFile = new PrivateKeyFile(privateKeyStream);
				var authMethod = new PrivateKeyAuthenticationMethod(username, keyFile);
				var connectionInfo = new ConnectionInfo(hostname, 22, username, authMethod);

				using var client = new SshClient(connectionInfo);
				client.Connect();

				var command = client.CreateCommand("vcgencmd measure_temp");
				var result = command.Execute()?.Trim();

				client.Disconnect();

				if (string.IsNullOrWhiteSpace(result))
					return (double?)null;

				var tempPart = result.Replace("temp=", "").Replace("'C", "");

				return double.TryParse(tempPart, CultureInfo.InvariantCulture, out var temp)
					? temp
					: (double?)null;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message,ex);
				return null;
			}
		});
	}

}
