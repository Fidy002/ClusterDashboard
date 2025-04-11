using ClusterDashboard.Application.Interfaces;
using ClusterDashboard.Application.Models;
using Microsoft.AspNetCore.Components;

namespace ClusterDashboard.Presentation.Components.Tiles;

public partial class PiTemperatures : ComponentBase
{
	[Inject] private INodeService NodeService { get; set; } = default!;

	private List<NodeInformation> _nodes = new();
	private bool _isLoading = true;


	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;
		try
		{
			_nodes = await NodeService.GetNodeInformation();
		}
		finally
		{
			_isLoading = false;
			StateHasChanged();
		}
	}

	protected override async Task OnInitializedAsync()
	{

	}
}

