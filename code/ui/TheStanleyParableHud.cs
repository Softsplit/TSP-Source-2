using Sandbox;
using Sandbox.UI;

[Library]
public partial class TheStanleyParableHud : HudEntity<RootPanel>
{
	public TheStanleyParableHud()
	{
		if ( !Game.IsClient )
			return;

		RootPanel.StyleSheet.Load( "/Styles/thestanleyparable.scss" );

		RootPanel.AddChild<Chat>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<VoiceSpeaker>();
		RootPanel.AddChild<Crosshair>();
	}
}
