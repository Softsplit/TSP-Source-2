using System;
using System.Linq;
using Sandbox;
using Nest.Components;

public class TriggerVolume : NestInput, Component.ITriggerListener
{
	[Flags]
	public enum TriggerType
	{
		Enter = 1,
		Stay = 2,
		Exit = 4
	}

	[Property]
	public TriggerType TriggerEvent { get; set; } = TriggerType.Enter;

	[Property]
	public int TagMask { get; set; } = -1;

	[Property]
	private string[] _tagValues { get; set; }

	[Property]
	public bool IsEnterAndExit => TriggerEvent == (TriggerType.Enter | TriggerType.Exit);

	[Property]
	public bool IsEnterAndStay => TriggerEvent == (TriggerType.Enter | TriggerType.Stay);

	[Property]
	public bool IsExitAndStay => TriggerEvent == (TriggerType.Stay | TriggerType.Exit);

	public void OnTriggerEnter( Collider other )
	{
			if ( IsEnterAndExit || IsEnterAndStay )
			{
				SetValue( value: true );
			}

			Invoke( TriggerType.Enter );
	}

	public void OnTriggerExit( Collider other )
	{
			if ( IsEnterAndExit )
			{
				SetValue( value: false );
			}

			Invoke( TriggerType.Exit );
	}

	public void Invoke( TriggerType type )
	{
		if ( (TriggerEvent & type) != 0 )
		{
			Invoke();
		}
	}

	public void SetValue( bool value )
	{
		SetBool( value );
	}
}
