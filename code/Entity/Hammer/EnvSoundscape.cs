using Editor;
using Sandbox;

/// <summary>
/// An entity to control sound in an area. The active soundscape at any time is the last one that had line-of-sight to the player, and was within the radius.
/// </summary>
[Library("env_soundscape"), HammerEntity]
[EditorSprite("editor/env_soundscape.vmat")]
[Title("Soundscape"), Category("Environment"), Icon("speaker")]
public partial class EnvSoundscape : Entity
{

}