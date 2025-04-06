namespace ClusterDashboard.Configuration;

public class KubernetesOptions
{
	public string Username { get; set; }

	public string MasterNodeHostname { get; set; }

	public string[] WorkerNodeHostnames { get; set; }

	public string PrivateKey { get; set; }
}
