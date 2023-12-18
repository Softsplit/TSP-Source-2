using System;
using Sandbox;

public class MainCamera : CameraComponent
{
	[Property]
	public static CameraComponent Camera { get; set; }

	[Property]
	public static Transform RaycastTarget { get; set; }

	[Property]
	public static float BlurValue { get; set; }

	/*
	[Property]
	private MobileBlur blur { get; set; }
	*/

	[Property]
	public static bool UseVicinityRenderingOnly { get; set; }

	/*
	[Property]
	public static EasyPortal[] Portals { get; set; }
	*/

	[Property]
	private BooleanConfigurable DefaultConfigurationConfigurable { get; set; }

	[Property]
	private IntConfigurable TSPVersion { get; set; }

	[Property]
	private Transform raycastTarget { get; set; }

	[Property]
	private GameObject[] portalGameObjects { get; set; }

	/*
	[Property]
	private EasyPortal[] portals { get; set; }
	*/

	protected override void OnAwake()
	{
		if ( raycastTarget != null )
		{
			RaycastTarget = raycastTarget;
		}
		// UpdatePortals();
		// GameMaster.OnPause += OnPause;
		// GameMaster.OnResume += OnResume;
		// SceneManager.sceneLoaded += OnSceneLoaded;
	}

	protected override void OnStart()
	{
		Camera = Components.Get<CameraComponent>();
		// blur.enabled = false;
	}

	/*
	private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
	{
		OnResume();
	}
	*/

	private void OnPause()
	{
		HandleBlur();
	}

	private void OnResume()
	{
		// blur.enabled = false;
		BlurValue = 0f;
	}

	protected override void OnDestroy()
	{
		// GameMaster.OnPause -= OnPause;
		// GameMaster.OnResume -= OnResume;
		// SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void HandleBlur()
	{
		if ( TSPVersion.GetIntValue() == 1 )
		{
			// blur.material.SetColor( "_Tint", Color.Red );
		}
		else
		{
			// blur.material.SetColor( "_Tint", Color.White );
		}
		// blur.BlurAmount = 0f;
		// blur.enabled = true;
	}

	private void Update()
	{
		/*
		if ( blur.enabled )
		{
			float num = MathX.Lerp( blur.BlurAmount, 1.5f, Time.Delta * 2f );
			blur.BlurAmount = num;
			BlurValue = num;
		}
		*/
	}

	/*
	public void UpdatePortals()
	{
		UseVicinityRenderingOnly = DefaultConfigurationConfigurable.GetBooleanValue();
		Portals = GameObject.FindObjectsOfType<EasyPortal>();
		portalGameObjects = new GameObject[Portals.Length];
		for ( int i = 0; i < Portals.Length; i++ )
		{
			portalGameObjects[i] = Portals[i].gameObject;
		}
	}

	private void SortArray()
	{
		Array.Sort( Portals );
		portals = Portals;
	}

	private void OnPreCull()
	{
		for ( int i = 0; i < Portals.Length; i++ )
		{
			if ( !Portals[i].disabled )
			{
				Portals[i].Render();
			}
		}
		for ( int j = 0; j < Portals.Length; j++ )
		{
			if ( !Portals[j].disabled )
			{
				Portals[j].PostPortalRender();
			}
		}
	}
	*/
}
