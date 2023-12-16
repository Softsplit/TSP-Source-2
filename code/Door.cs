using Sandbox;

public sealed class Door : Component
{
	[Property] public bool Opened { get; set; }
	[Property] public bool Locked { get; set; }
	
	float _openFraction;
	
	[Property, Group("Controls")] 
	public float SecondsToOpen { get; set; }
	
	[Property, Group("Controls")] 
	public Curve Easing { get; set; }

	/// <summary>
	/// Start point
	/// </summary>
	[Property, Group("Controls")] 
	public Angles RotationA { get; set; }
	
	/// <summary>
	/// End point
	/// </summary>
	[Property, Group("Controls")] 
	public Angles RotationB { get; set; }

	protected override void OnFixedUpdate()
	{
		_openFraction = _openFraction.Approach( Opened ? 1 : 0, Time.Delta / SecondsToOpen );
		var easedFraction = Easing.Evaluate( _openFraction );

		var newRotation = RotationA.LerpTo( RotationB, easedFraction );
		
		Transform.Rotation = newRotation;
	}
	
	public void Open()
	{
		Opened = true;
	}
	
	public void Close()
	{
		Opened = false;
	}
}
