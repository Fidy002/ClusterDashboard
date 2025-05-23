﻿using ClusterDashboard.Application.Interfaces;
using ClusterDashboard.Application.Models;
using ClusterDashboard.Configuration;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace ClusterDashboard.Infrastructure.Services;

public class NodeService : INodeService
{
	private KubernetesOptions _kubernetesOptions;
	private readonly IDeviceTemperatureService _deviceTemperatureService;

	public NodeService(IOptions<KubernetesOptions> options, IDeviceTemperatureService deviceTemperatureService)
	{
		_kubernetesOptions = options.Value;
		_deviceTemperatureService = deviceTemperatureService;
	}

	public async Task<List<NodeInformation>> GetNodeInformation()
	{
		var nodeInformation = new List<NodeInformation>();
		var username = _kubernetesOptions.Username;
		var privateKey = _kubernetesOptions.PrivateKey;
		var masterNodeName = _kubernetesOptions.MasterNodeHostname;

		await AddNodeInformation(nodeInformation, username, privateKey, masterNodeName);

		foreach(var host in _kubernetesOptions.WorkerNodeHostnames)
		{
			await AddNodeInformation(nodeInformation, username, privateKey, host);
		}
		return nodeInformation;
	}

	private async Task AddNodeInformation(List<NodeInformation> nodeInformation, string username, string privateKey, string host)
	{
		var deviceTemperature = await _deviceTemperatureService.GetTemperatureAsync(host, username, privateKey);
		if (deviceTemperature == null)
		{
			// TODO: Log Error
			return;
		}
		nodeInformation.Add(new() { NodeName = host, Temperature = deviceTemperature.Value });
	}
}
