using Sandbox;

public class FirstPerson : Component
{
	[Property] public SkinnedModelRenderer Target { get; set; }


	protected override void OnStart()
	{
		// Make target invisible.
		Target.SceneModel.Flags.IsOpaque = false;
	}

	protected override void OnDisabled()
	{
		if ( Target != null ) Target.SceneModel.Flags.IsOpaque = true;
	}
}
