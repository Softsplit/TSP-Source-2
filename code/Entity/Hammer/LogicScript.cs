using Editor;
using Sandbox;

/// <summary>
/// An entity that acts as a container for scripts
/// </summary>
[Library("logic_script"), HammerEntity]
[EditorSprite("editor/logic_script.vmat"), VisGroup(VisGroup.Logic)]
[Title("Script"), Category("Logic"), Icon("description")]
public partial class LogicScript : Entity
{

}