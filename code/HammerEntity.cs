using System.Collections.Generic;
using System.Reflection;
using Sandbox;
using Nest.Components;

public class HammerEntity : Component
{
	[Property] public List<HammerConnection> connections { get; set; } = new List<HammerConnection>();

	[Property] public List<HammerConnection> expandedConnections { get; set; } = new List<HammerConnection>();

	public GameObject parent;

	[Property] public bool isEnabled { get; set; } = true;

	private bool sorted;

	protected void FireOutput( Outputs output )
	{
		FireOutput( output, "", float.NaN );
	}

	protected void FireOutput( Outputs output, string parameterOverride )
	{
		FireOutput( output, parameterOverride, float.NaN );
	}

	protected void FireOutput( Outputs output, float parameterFloat )
	{
		FireOutput( output, "", parameterFloat );
	}

	protected void FireOutput( Outputs output, string parameterString, float parameterFloat )
	{
		if ( !sorted )
		{
			SortOutputsByDelay();
			sorted = true;
		}
		for ( int i = 0; i < expandedConnections.Count; i++ )
		{
			if ( expandedConnections[i].output != output || expandedConnections[i].nestInput != null )
			{
				continue;
			}
			if ( parameterString != "" )
			{
				/*
				if ( expandedConnections[i].nestInput.CurrentEventType == NestInput.EventType.String && expandedConnections[i].nestInput._eventString.GetPersistentEventCount() > 0 )
				{
					object persistentTarget = expandedConnections[i].nestInput._eventString.GetPersistentTarget( 0 );
					MethodInfo method = persistentTarget.GetType().GetMethod( expandedConnections[i].nestInput._eventString.GetPersistentMethodName( 0 ), new Type[1] { typeof( string ) } );
					expandedConnections[i].nestInput._eventString = new NestInput.StringEvent();
					UnityAction<string> call = Delegate.CreateDelegate( typeof( UnityAction<string> ), persistentTarget, method ) as UnityAction<string>;
					expandedConnections[i].nestInput._eventString.AddListener( call );
					
				}
				*/

					expandedConnections[i].nestInput._parameterString = parameterString;
			}
			if ( !float.IsNaN( parameterFloat ) )
			{
				/*
				if ( expandedConnections[i].nestInput.CurrentEventType == NestInput.EventType.Float && expandedConnections[i].nestInput._eventValue.GetPersistentEventCount() > 0 )
				{
					object persistentTarget2 = expandedConnections[i].nestInput._eventValue.GetPersistentTarget( 0 );
					MethodInfo method2 = persistentTarget2.GetType().GetMethod( expandedConnections[i].nestInput._eventValue.GetPersistentMethodName( 0 ), new Type[1] { typeof( float ) } );
					expandedConnections[i].nestInput._eventValue = new NestInput.ValueEvent();
					UnityAction<float> call2 = Delegate.CreateDelegate( typeof( UnityAction<float> ), persistentTarget2, method2 ) as UnityAction<float>;
					expandedConnections[i].nestInput._eventValue.AddListener( call2 );
				}
				*/
				expandedConnections[i].nestInput._parameterFloat = parameterFloat;
			}
			expandedConnections[i].nestInput.Invoke();
		}
	}

	private void SortOutputsByDelay()
	{
		List<HammerConnection> list = new List<HammerConnection>();
		while ( expandedConnections.Count > 0 )
		{
			float num = float.MaxValue;
			int num2 = -1;
			for ( int i = 0; i < expandedConnections.Count; i++ )
			{
				if ( expandedConnections[i].delay < num )
				{
					num = expandedConnections[i].delay;
					num2 = i;
				}
			}
			if ( num2 != -1 )
			{
				list.Add( expandedConnections[num2] );
				expandedConnections.Remove( expandedConnections[num2] );
			}
		}
		expandedConnections = list;
	}

	protected override void OnValidate()
	{
		int i;
		for ( i = 0; i < expandedConnections.Count; i++ )
		{
			// new List<Component>( GetComponents<Component>() ).FindIndex( ( Component x ) => x == expandedConnections[i].nestInput );
		}
	}

	/*
	private IEnumerator LogOutput( HammerConnection connection, string output, string filter )
	{
		yield return new WaitForGameSeconds( connection.nestInput.Delay );
		string text = " ::: ";
		string text2 = "                              ";
		string text3 = text + "              OUTPUT              " + text;
		string text4 = ((!(connection.recipientObject == null)) ? connection.recipientObject.name : "DESTROYED");
		string text5 = ((!(connection.nestInput._parameterString != "")) ? connection.nestInput._parameterFloat.ToString() : connection.nestInput._parameterString);
		text3 = text3 + base.name + text2.Substring( base.name.Length ) + text;
		text3 = text3 + output + text2.Substring( output.Length ) + text;
		text3 = text3 + text4 + text2.Substring( text4.Length ) + text;
		text3 = text3 + connection.input.ToString() + text2.Substring( connection.input.ToString().Length ) + text;
		text3 = text3 + text5 + text2.Substring( text5.Length ) + text;
		text3 += connection.nestInput.Delay;
		if ( !(filter == "") )
		{
			text3.Contains( filter );
		}
	}
	*/

	public virtual void Use()
	{
	}

	public virtual void Input_Enable()
	{
		
	}

	public virtual void Input_Disable()
	{
		isEnabled = false;
	}

	public virtual void Input_Kill()
	{
		GameObject.Enabled = false;
	}

	public void Input_FireUser1()
	{
		FireOutput( Outputs.OnUser1 );
	}

	public void Input_FireUser2()
	{
		FireOutput( Outputs.OnUser2 );
	}

	public void Input_FireUser3()
	{
		FireOutput( Outputs.OnUser3 );
	}

	public void Input_FireUser4()
	{
		FireOutput( Outputs.OnUser4 );
	}
}
