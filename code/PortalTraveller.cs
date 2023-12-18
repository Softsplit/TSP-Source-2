using Sandbox;
using System.Collections.Generic;
using System.Numerics;

public class PortalTraveller : Component
{
	[Property]
	public GameObject graphicsObject { get; set; }

	[Property]
	public bool InstantiateClone { get; set; }

	[Property]
	public GameObject graphicsClone { get; set; }

	[Property]
	public Vector3 previousOffsetFromPortal { get; set; }

	[Property]
	public Material[] originalMaterials { get; set; }

	[Property]
	public Material[] cloneMaterials { get; set; }

	public virtual void Teleport( Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot )
	{
		base.Transform.Position = pos;
		base.Transform.Rotation = rot;
	}

	public virtual void EnterPortalThreshold()
	{
		if ( InstantiateClone )
		{
			if ( graphicsClone == null )
			{
				graphicsClone = SceneUtility.Instantiate( graphicsObject );
				// graphicsClone.Transform.Parent = graphicsObject.Transform.Parent;
				graphicsClone.Transform.LocalScale = graphicsObject.Transform.LocalScale;
				originalMaterials = GetMaterials( graphicsObject );
				cloneMaterials = GetMaterials( graphicsClone );
			}
			else
			{
				graphicsClone.Enabled = true;
			}
		}
	}

	public virtual void ExitPortalThreshold()
	{
		if ( InstantiateClone )
		{
			graphicsClone.Enabled = false;
			for ( int i = 0; i < originalMaterials.Length; i++ )
			{
				originalMaterials[i].Set( "sliceNormal", Vector3.Zero );
			}
		}
	}

	public void SetSliceOffsetDst( float dst, bool clone )
	{
		if ( !InstantiateClone )
		{
			return;
		}
		for ( int i = 0; i < originalMaterials.Length; i++ )
		{
			if ( clone )
			{
				cloneMaterials[i].Set( "sliceOffsetDst", dst );
			}
			else
			{
				originalMaterials[i].Set( "sliceOffsetDst", dst );
			}
		}
	}

	private Material[] GetMaterials( GameObject g )
	{
		ModelRenderer[] componentsInChildren = g.Components.GetInChildren<ModelRenderer[]>();
		List<Material> list = new List<Material>();
		ModelRenderer[] array = componentsInChildren;
		for ( int i = 0; i < array.Length; i++ )
		{
			/*
			Material[] materials = array[i].Materials;
			foreach ( Material item in materials )
			{
				list.Add( item );
			}
			*/
		}
		return list.ToArray();
	}
}
