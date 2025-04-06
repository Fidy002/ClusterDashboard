namespace ClusterDashboard.Infrastructure.Services;

using Renci.SshNet;
using ClusterDashboard.Application.Interfaces;
using System.Text;

public class SshDeviceTemperatureService : IDeviceTemperatureService
{
	public Task<double?> GetTemperatureAsync(string hostname, string username, string privateKey)
	{
		try
		{
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
				return Task.FromResult<double?>(null);

			var tempPart = result.Replace("temp=", "").Replace("'C", "");

			if (double.TryParse(tempPart, System.Globalization.CultureInfo.InvariantCulture, out var temp))
				return Task.FromResult<double?>(temp);

			return Task.FromResult<double?>(null);
		}
		catch
		{
			return Task.FromResult<double?>(null);
		}
	}
}
