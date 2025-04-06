using ClusterDashboard.Application.Interfaces;

namespace ClusterDashboard.Presentation.Components.Tiles;

partial class PiTemperatures
{
	private readonly INodeService _nodeService;

	public PiTemperatures(INodeService nodeService)
	{
		_nodeService = nodeService;
	}


}
