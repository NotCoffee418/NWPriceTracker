namespace NWPriceTracker.Client.Data
{
	public class InstallInfo
	{
		public enum Status
		{
			Undefined,
			Installing,
			Installed,
			Error,
		}

		/// <summary>
		/// Name of the installer used by SignalR
		/// </summary>
		public string InstallerName { get; init; }

		/// <summary>
		/// Visible description on the install page
		/// </summary>
		public string Description { get; init; }

		/// <summary>
		/// Current status of the installation
		/// </summary>
		public Status InstallStatus { get; set; } = Status.Undefined;

		/// <summary>
		/// Any status message, optional
		/// </summary>
		public string StatusMessage { get; set; }
	}
}
