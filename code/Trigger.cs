using Sandbox;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace TSPS2;

public class Trigger : Component, Component.ITriggerListener
{

	// Start touch
	public delegate Task StartTouchFunc( GameObject other );
	
	[Property, Group("Start Touch")] public List<StartTouchFunc> StartTouch { get; set; }
	[Property, Group("Start Touch")] public List<GameObject> StartTouchTarget { get; set; }

	// End touch
	public delegate Task EndTouchFunc( GameObject other );
	
	[Property, Group("End Touch")] public List<EndTouchFunc> EndTouch { get; set; }
	[Property, Group("End Touch")] public List<GameObject> EndTouchTarget { get; set; }

	public void OnTriggerEnter( Collider other )
	{
		if ( other.Static )
			return;
		
		for ( var i = 0; i < StartTouch.Count; i++ )
		{
			// Run action graphs
			_ = StartTouch[i]?.Invoke( StartTouchTarget[i] );
		}
	}
	
	public void OnTriggerExit( Collider other )
	{
		if ( other.Static )
			return;
		
		for ( var i = 0; i < EndTouch.Count; i++ )
		{
			// Run action graphs
			_ = EndTouch[i]?.Invoke( EndTouchTarget[i] );
		}
	}
}
