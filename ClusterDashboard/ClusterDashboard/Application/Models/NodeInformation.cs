namespace ClusterDashboard.Application.Models;

public record NodeInformation
{
	public required string NodeName { get; set; }

	public required double Temperature { get; set; }

}
