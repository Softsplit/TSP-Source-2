using Sandbox;

public partial class Carriable : BaseCarriable, IUse
{
	public bool OnUse( Entity user )
	{
		return false;
	}

	public virtual bool IsUsable( Entity user )
	{
		return Owner == null;
	}
}
