using Sandbox;
using Editor;

/// <summary>
/// A prop that can be placed in hierarchy and can play animations. It can also be configured to break when it takes enough damage. Note that the health of the object will be overridden by the health inside the model, to ensure consistent health game-wide. If the model used by the prop is configured to be used as a prop_physics (i.e. it should be physically simulated) then it CANNOT be used as a prop_dynamic. Upon level load it will display a warning in the console and remove itself. Use a prop_physics instead.
/// </summary>
[Library("prop_dynamic"), HammerEntity]
[Model(Archetypes = ModelArchetype.animated_model), RenderFields, VisGroup(VisGroup.Dynamic)]
[Title("Dynamic"), Category("Prop"), Icon("animation")]
public partial class PropDynamic : Entity
{

}