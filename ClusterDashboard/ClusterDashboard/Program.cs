using ClusterDashboard.Application.Interfaces;
using ClusterDashboard.Configuration;
using ClusterDashboard.Presentation.Components;
using ClusterDashboard.Infrastructure.Services;
using MudBlazor.Services;
using ClusterDashboard.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveWebAssemblyComponents()
	.AddInteractiveServerComponents();

builder.Services.AddMudServices();

// options
builder.Services.Configure<KubernetesOptions>(
	builder.Configuration.GetSection("Kubernetes"));

//Services
builder.Services.AddTransient<IDeviceTemperatureService, DeviceTemperatureService>();
builder.Services.AddScoped<INodeService, NodeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveWebAssemblyRenderMode()
	.AddInteractiveServerRenderMode();
app.Run();
