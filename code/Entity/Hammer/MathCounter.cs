using Editor;
using Sandbox;

/// <summary>
/// Holds a numeric value and performs arithmetic operations upon it. If either the minimum or maximum legal value is nonzero, OutValue will be clamped to the legal range, and the OnHitMin/OnHitMax outputs will be fired at the appropriate times. If both min and max are set to zero, no clamping is performed and only the OutValue output will be fired.
/// </summary>
[Library("math_counter"), HammerEntity]
[EditorSprite("editor/math_counter.vmat")]
[Title("Counter"), Category("Miscellaneous"), Icon("calculate")]
public partial class MathCounter : Entity
{

}
