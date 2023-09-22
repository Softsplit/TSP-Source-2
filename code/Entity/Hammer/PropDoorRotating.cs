using Sandbox;
using Editor;

/// <summary>
/// An entity used to place a door in the world.
/// </summary>
[Library("prop_door_rotating")]
[HammerEntity, SupportsSolid]
[Model(Archetypes = ModelArchetype.animated_model)]
[RenderFields, VisGroup(VisGroup.Dynamic)]
[Title("Door Rotating"), Category("Prop"), Icon("door_front")]
public partial class PropDoorRotating : Entity
{

}