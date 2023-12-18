using Sandbox;
using System.Collections.Generic;

public class TriggerMultiple : HammerEntity, Component.ITriggerListener
{
	[Property]
	public bool onceOnly { get; set; }

	private List<Collider> touchingColliders = new List<Collider>();

	protected Rigidbody _body;

	protected Collider _collider;

	protected override void OnAwake()
	{
		_body = Components.Get<Rigidbody>();
		_collider = Components.Get<Collider>();
	}

	public void OnTriggerEnter( Collider col )
	{
		/*
		if ( col.CompareTag( "Player" ) && !touchingColliders.Contains( col ) )
		{
			_ = isEnabled;
			touchingColliders.Add( col );
			StartTouch();
		}
		*/
	}

	public void OnTriggerExit( Collider col )
	{
		if ( touchingColliders.Contains( col ) )
		{
			touchingColliders.Remove( col );
			if ( isEnabled )
			{
				FireOutput( Outputs.OnEndTouch );
			}
		}
	}

	public override void Input_Enable()
	{
		base.Input_Enable();
		for ( int i = 0; i < touchingColliders.Count; i++ )
		{
			/*
			if ( touchingColliders[i].CompareTag( "Player" ) )
			{
				StartTouch();
			}
			*/
		}
	}

	protected virtual void StartTouch()
	{
		if ( isEnabled )
		{
			FireOutput( Outputs.OnStartTouch );
			FireOutput( Outputs.OnTrigger );
			if ( onceOnly )
			{
				_body.Destroy();
				_collider.Destroy();
			}
		}
	}
}
