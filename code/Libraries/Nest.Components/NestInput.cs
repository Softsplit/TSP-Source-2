using System;
using Sandbox;
using Nest.Util;
namespace Nest.Components;

public class NestInput : Component
{
	/*
	[Serializable]
	public class ValueEvent : UnityEvent<float>
	{
	}

	[Serializable]
	public class StringEvent : UnityEvent<string>
	{
	}
	*/

	[Flags]
	public enum EventType
	{
		Trigger = 1,
		Bool = 2,
		Float = 4,
		Toggle = 0x10,
		String = 0x20
	}

	[Property]
	private EventType _eventType { get; set; } = EventType.Trigger;

	[Property]
	private float _eventOffValue { get; set; }

	[Property]
	private float _eventOnValue { get; set; } = 1f;

	/*
	[Property]
	private SerializableDictionaryBase<string, UnityEventBase> _events { get; set; }

	[Property]
	public UnityEvent _event { get; set; }

	[Property]
	private UnityEvent _event2 { get; set; }
	*/

	// [Property]
	// public ValueEvent _eventValue { get; set; }

	[Property]
	public float _parameterFloat { get; set; }

	// [Property]
	// public StringEvent _eventString { get; set; }

	[Property]
	public string _parameterString { get; set; }

	[Property]
	public bool FireOnce { get; set; }

	[Property]
	private FloatInterpolator.Config _interpolation { get; set; }

	private bool _fired;

	[Property]
	public float Delay { get; set; }

	protected FloatInterpolator Value;

	[Property]
	public bool HasFired => _fired;

	public EventType CurrentEventType
	{
		get
		{
			return _eventType;
		}
		set
		{
			_eventType = value;
		}
	}

	/*
	public UnityEventBase CurrentEvent
	{
		get
		{
			switch ( _eventType )
			{
				case EventType.Trigger:
					return _event;
				case EventType.Float:
					return _eventValue;
				case EventType.Bool:
				case EventType.Toggle:
					if ( Value.CurrentValue < 0.01f )
					{
						return _event;
					}
					if ( Value.CurrentValue > 0.99f )
					{
						return _event2;
					}
					break;
				case EventType.String:
					return _eventString;
			}
			return _event;
		}
	}
	*/

	public bool EventPosition
	{
		get
		{
			return MathF.Abs( Value.CurrentValue ) < 0.01f;
		}
		set
		{
			Value.CurrentValue = (value ? 1 : 0);
		}
	}

	public event CastEvent EventFired;

	public virtual void Start()
	{
		Value = new FloatInterpolator( _eventOffValue, _eventOnValue, _interpolation );
	}

	public void Invoke()
	{
		if ( (!FireOnce || !_fired) && base.Enabled )
		{
			_fired = true;
			if ( Delay > 0f )
			{
				// StartCoroutine( InvokeDelay() );
			}
			else
			{
				InvokeEvent();
			}
		}
	}

	/*
	private IEnumerator InvokeDelay()
	{
		yield return new WaitForGameSeconds( Delay );
		InvokeEvent();
	}
	*/

	public void InvokeEvent()
	{
		switch ( _eventType )
		{
			case EventType.Trigger:
				// _event.Invoke();
				break;
			case EventType.Bool:
				if ( MathF.Abs( Value.CurrentValue ) < 0.01f )
				{
					// _event2.Invoke();
				}
				else if ( MathF.Abs( Value.CurrentValue ) - 1f < 0.01f )
				{
					// _event.Invoke();
				}
				break;
			case EventType.Float:
				// _eventValue.Invoke( _parameterFloat );
				break;
			case EventType.Toggle:
				Value.CurrentValue = 1f - Value.CurrentValue;
				if ( MathF.Abs( Value.CurrentValue ) < 0.01f )
				{
					// _event2.Invoke();
				}
				else
				{
					// _event.Invoke();
				}
				break;
			case EventType.String:
				// _eventString.Invoke( _parameterString );
				break;
		}
		if ( this.EventFired != null )
		{
			this.EventFired( this );
		}
	}

	public int[] FindGameObjectsReferences()
	{
		float currentValue = Value.CurrentValue;
		int[] array = null;
		Value.CurrentValue = 0f;
		// int num = CurrentEvent.GetPersistentEventCount();
		if ( _eventType == EventType.Bool || _eventType == EventType.Toggle )
		{
			// num += _event2.GetPersistentEventCount();
		}
		// array = new int[num];
		for ( int i = 0; /* i < num */; i++ )
		{
			GameObject @object = null;
			/*
			if ( i > CurrentEvent.GetPersistentEventCount() )
			{
				@object = _event2.GetPersistentTarget( i - CurrentEvent.GetPersistentEventCount() );
			}
			else if ( CurrentEvent.GetPersistentEventCount() > i )
			{
				@object = CurrentEvent.GetPersistentTarget( i );
			}
			else if ( _event2.GetPersistentEventCount() > i )
			{
				@object = _event2.GetPersistentTarget( i );
			}
			if ( @object is GameObject )
			{
				array[i] = @object.GetInstanceID();
			}
			else if ( @object is Component )
			{
				array[i] = ((Component)@object).gameObject.GetInstanceID();
			}
			*/
		}
		Value.CurrentValue = currentValue;
		return array;
	}

	public void SetBool( bool value )
	{
		Value.TargetValue = (value ? _eventOnValue : _eventOffValue);
		Value.CurrentValue = Value.TargetValue;
	}
}
