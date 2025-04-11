using ClusterDashboard.Application.Models;

namespace ClusterDashboard.Application.Interfaces;

public interface INodeService
{
	Task<List<NodeInformation>> GetNodeInformation();
}
