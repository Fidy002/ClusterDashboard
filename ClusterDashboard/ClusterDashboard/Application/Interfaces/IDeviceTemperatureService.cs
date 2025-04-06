namespace ClusterDashboard.Application.Interfaces;

public interface IDeviceTemperatureService
{
	Task<double?> GetTemperatureAsync(string hostname, string username, string privateKey);
}
