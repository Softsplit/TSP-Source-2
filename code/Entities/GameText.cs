using Editor;
using Sandbox;

/// <summary>
/// An entity that displays text on player's screens.
/// </summary>
[Library("game_text"), HammerEntity]
[EditorSprite("editor/game_text.vmat")]
[Title("Text"), Category("Game"), Icon("text_fields")]
public partial class GameText : Entity
{
}