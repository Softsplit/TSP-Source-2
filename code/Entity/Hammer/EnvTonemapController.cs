using Editor;
using Sandbox;

/// <summary>
/// An entity that controls the HDR tonemapping for the player. Think of it as a method of controlling the exposure of the player's eyes.
/// </summary>
[Library("env_tonemap_controller"), HammerEntity]
[EditorSprite("editor/env_tonemap_controller.vmat")]
[Title("Tonemap Controller"), Category("Environment"), Icon("gradient")]
public partial class EnvTonemapController : Entity
{

}