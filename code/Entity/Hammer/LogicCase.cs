using Editor;
using Sandbox;

/// <summary>
/// Compares an input to up to 16 preset values. If the input value is the same as any of the preset values, an output corresponding to that value is fired. <br/>
/// <br/>
/// For example: if Case01 is set to 2 and Case02 is set to 5, and the input value is 5, the OnCase02 output will be fired. <br/>
/// <br/>
/// This entity can also be used to select from a number of random targets via the PickRandom input. One of the OnCase outputs that is connected to another entity will be picked at random and fired.
/// </summary>
[Library("logic_case"), HammerEntity]
[EditorSprite("editor/logic_case.vmat"), VisGroup(VisGroup.Logic)]
[Title("Case"), Category("Logic"), Icon("login")]
public partial class LogicCase : Entity
{

}