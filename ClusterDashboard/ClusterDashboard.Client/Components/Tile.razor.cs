using Microsoft.AspNetCore.Components;

namespace ClusterDashboard.Client.Components;

partial class Tile
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }
}
