using Sandbox;

public sealed class NonRendererFootstepType : Component
{
	[Property]
	private StanleyData.FootstepSounds footstepType { get; set; }

	[Property]
	private bool forceOverrideMaterial { get; set; }

	public StanleyData.FootstepSounds FootstepType => footstepType;

	public bool ForceOverrideMaterial => forceOverrideMaterial;
}
