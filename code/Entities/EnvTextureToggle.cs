using Editor;
using Sandbox;

/// <summary>
/// An entity that allows you to change the textures on other brush-built entities.
/// </summary>
[Library("env_texturetoggle"), HammerEntity]
[EditorSprite("editor/env_texturetoggle.vmat")]
[Title("Texture Toggle"), Category("Environment"), Icon("toggle_on")]
public partial class EnvTexturetoggle : Entity
{
	[Property]
	public string Target { get; set; } = "";

	[Input]
	public void IncrementTextureIndex()
	{
	
	}

    [Input]
	public void SetTextureIndex( int index )
	{
	
	}
}