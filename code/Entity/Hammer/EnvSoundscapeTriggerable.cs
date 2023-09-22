using Editor;
using Sandbox;

/// <summary>
/// An entity that works like env_soundscape except that it works in conjunction with trigger_soundscape to determine when a player hears it.
/// </summary>
[Library("env_soundscape_triggerable"), HammerEntity]
[EditorSprite("editor/env_soundscape.vmat")]
[Title("Soundscape Triggerable"), Category("Environment"), Icon("speaker")]
public partial class EnvSoundscapeTriggerable : Entity
{

}