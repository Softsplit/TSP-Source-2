using Editor;
using Sandbox;

/// <summary>
/// A camera entity that controls the player's view. While it's active, the player will see out of the camera.
/// </summary>
[Library("point_viewcontrol"), HammerEntity]
[EditorModel("models/editor/camera.vmdl")]
[Title("View Control"), Category("Point"), Icon("videocam")]
public partial class PointViewcontrol : Entity
{

}