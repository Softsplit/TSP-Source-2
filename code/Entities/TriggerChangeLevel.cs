using Editor;
using Sandbox;

/// <summary>
/// A simple trigger volume that teleports entities that touch it.
/// </summary>
[Library("trigger_changelevel")]
[HammerEntity, Solid]
[Title("Change Level"), Category("Trigger"), Icon("auto_fix_normal")]
public partial class TriggerChangelevel : BaseTrigger
{

}