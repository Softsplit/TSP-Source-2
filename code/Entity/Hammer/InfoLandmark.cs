using Editor;
using Sandbox;

/// <summary>
/// An entity that acts as a landmark for transitions to another level. There should be a corresponding info_landmark entity in the next map. Entities will be transitioned to the next level relative to the info_landmark entities.
/// </summary>
[Library("info_landmark"), HammerEntity]
[EditorSprite("editor/info_lighting.vmat")]
[Title("Landmark"), Category("Information"), Icon("flag")]
public partial class InfoLandmark : Entity
{

}