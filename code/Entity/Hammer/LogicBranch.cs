using Editor;
using Sandbox;

/// <summary>
/// Tests a boolean value and fires an output based on whether the value is true or false. Use this entity to branch between two potential sets of events.
/// </summary>
[Library("logic_branch"), HammerEntity]
[EditorSprite("editor/logic_branch.vmat"), VisGroup(VisGroup.Logic)]
[Title("Branch"), Category("Logic"), Icon("alt_route")]
public partial class LogicBranch : Entity
{

}
