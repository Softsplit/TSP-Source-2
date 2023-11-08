using Sandbox;
using Sandbox.UI;

namespace TSPS2.UI;

public partial class TSPHud : HudEntity<TSPRootPanel>
{
	public static TSPHud Instance { get; set; }

	public TSPHud()
	{
		Instance = this;
	}

	[GameEvent.Client.Frame]
	public void OnHudChangeEnabled()
	{
		if (Enabled)
		{
			if (!RootPanel.IsVisible)
				RootPanel.Style.Set("display", "flex");
		}
		else
		{
			if (RootPanel.IsVisible)
				RootPanel.Style.Set("display", "none");
		}
	}

	[ConVar.Client("cl_drawhud")] public static bool Enabled { get; set; } = true;
}