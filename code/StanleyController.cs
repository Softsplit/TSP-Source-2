using System;
using System.Collections;
using System.Collections.Generic;
using Sandbox;

public class StanleyController : PortalTraveller
{
	public enum TeleportType
	{
		PlayerStart,
		PlayerStartMaster,
		TriggerTeleport
	}

	private static StanleyController _instance;

	[Property]
	public static Action<GameObject> OnInteract { get; set; }

	[Property]
	public static Vector3 StanleyPosition { get; set; }

	[Property]
	public static Action OnActuallyJumping { get; set; }

	[Property]
	public static Action OnTeleportToPlayerStart { get; set; }

	private CharacterController character;

	private Vector3 camParentOrigLocalPos;

	private float floatingStrength;

	private float floatingPos;

	private float viewPitch;

	private RotatingDoor lastHitRotatingDoor;

	/*
	[Property]
	public LayerMask clickLayers {  get; set; }

	[Property]
	public LayerMask groundedLayers { get; set; }
	*/

	[Property]
	public Texture fadeTexture { get; set; }

	private bool fading;

	private Color fadeColor = Color.Black;

	private bool fadeHold;

	private IEnumerator fadeCoroutine;

	private IEnumerator FarZCoroutine;

	// private AmplifyColorBase AColor;

	private Vector3 movementInput;

	private bool masterStartFound;

	[Property]
	public Transform spinner { get; set; }

	[Property]
	public bool motionFrozen { get; set; }

	[Property]
	public bool viewFrozen { get; set; }

	private StanleyData.FootstepSounds currentlyStandingOn = StanleyData.FootstepSounds.Silence;

	private GameObject currentlyStandingOnGameObject;

	private Material currentlyStandingOnMaterial;

	[Group( "Audio" ), Property]
	public float stepDistance { get; set; } = 0.3f;

	[Group( "Audio" ), Property]
	public float footstepVolume { get; set; } = 0.4f;

	[Property]
	private AudioCollection barkingCollection { get; set; }

	[Property]
	private AudioCollection keyboardCollection { get; set; }

	[Property]
	private AudioCollection[] footstepCollections { get; set; }

	/*
	[Property]
	private AudioSource footstepSource { get; set; }

	[NonSerialized]
	private AudioSource[] footstepSources;


	[Property]
	private AudioSource useSource { get; set; }
	*/

	[Group( "Movement" )]
	[Property]
	private float walkingSpeed { get; set; } = 3f;

	[Group( "Movement" )]
	[Property]
	private float runAcceleration { get; set; } = 14f;

	[Group( "Movement" )]
	[Property]
	private float runDeacceleration { get; set; } = 10f;

	[Group( "Movement" )]
	[Property]
	private float inAirMovementMultiplier { get; set; } = 0.7f;

	[Group( "Movement" )]
	[Property]
	private float crouchMovementMultiplier { get; set; } = 0.65f;

	[Group( "Audio Movement" )]
	[Property]
	[Range( 0f, 1f )]
	private float footstepsYMultiplier { get; set; } = 0.01f;

	[Group( "Audio Movement" )]
	[Property]
	[Range( 0f, 1f )]
	private float standingOnUpdateLimit { get; set; } = 0.5f;

	[Group( "Audio Movement" )]
	[Property]
	private bool playFootstepOnNewMaterial { get; set; } = true;

	private AudioCollection currentFootstepAudioCollection;

	[Group( "Gravity" )]
	[Property]
	private float maxGravity { get; set; } = -19f;

	[Group( "Gravity" )]
	[Property]
	[Range( 0f, 1f )]
	private float groundedGravityMultiplier { get; set; } = 0.1f;

	[Group( "Gravity" )]
	[Property]
	private float gravityFallAcceleration { get; set; } = 1f;

	[NonSerialized]
	private float gravityMultiplier;

	private float velocityAccumulation;

	[Group( "Controls" )]
	[Property]
	public float mouseSensitivityX { get; set; }

	[Group( "Controls" )]
	[Property]
	public float mouseSensitivityY { get; set; }

	[Group( "Controls" )]
	[Property]
	public float controllerSensitivityX { get; set; }

	[Group( "Controls" )]
	[Property]
	public float controllerSensitivityY { get; set; }

	[Group( "Controls" )]
	[Property]
	public bool yInvert { get; set; }

	[Group( "Controls" )]
	[Property]
	public float armReach { get; set; } = 1f;

	private Vector3 slidingDirection;

	private Vector3 hitNormal;

	private bool isSliding;

	[Group( "Crouch" )]
	[Property]
	private Transform cameraParent { get; set; }

	[Group( "Crouch" )]
	[Property]
	private float uncrouchedColliderHeight { get; set; } = 0.632f;

	[Group( "Crouch" )]
	[Property]
	private float crouchedColliderHeight { get; set; } = 0.316f;

	[Group( "Crouch" )]
	[Property]
	private float crouchSmoothing { get; set; } = 0.3f;

	[Group( "Crouch" )]
	[Property]
	private float characterHeightMultipler { get; set; } = 1f;

	private bool wasCrouching;

	[Property]
	private StanleyData stanleyData { get; set; }

	private Dictionary<int, AudioCollection> footstepDictionary = new Dictionary<int, AudioCollection>();

	private Vector3 movementGoal = Vector3.Zero;

	private Vector3 movement = Vector3.Zero;

	private MainCamera mainCamera;

	[Property]
	private FloatConfigurable fovSettingConfigurable { get; set; }

	private FloatConfigurable mouseSensitivityConfigurable;

	[Property]
	private FloatConfigurable controllerSensitivityConfigurable { get; set; }

	[Property]
	private BooleanConfigurable invertYConfigurable { get; set; }

	[Property]
	private BooleanConfigurable mouseWalkConfigurable { get; set; }

	[Property]
	private BooleanConfigurable mouseWalkToggleConfigurable { get; set; }

	private float inAirLimit = 12f;

	private bool grounded;

	private float inAirTimer;

	private bool outOfBoundsReported;

	[Property]
	public static Action OnOutOfBounds { get; set; }

	[Group( "Debug" )]
	[Property]
	private GameObject lightmapDebugUIPrefab { get; set; }

	private GameObject lightmapDebugUIInstance;

	private bool autoWalking;

	private bool frozenFromCommandMotion;

	private bool frozenFromCommandView;

	private float standingOnUpdateTimer;

	private const bool FOOTSTEP_TYPE_DEBUG = false;

	private const bool FOOTSTEP_TIMING_DEBUG = false;

	[Property]
	public static StanleyController Instance
	{
		get
		{
			if ( _instance == null )
			{
				// _instance = Singleton<GameMaster>.Instance?.RespawnStanley();
			}
			return _instance;
		}
	}

	[Property]
	public Transform Transformransform { get; private set; }

	[Property]
	public CameraComponent cam { get; private set; }

	[Property]
	public CameraComponent currentCam { get; private set; }

	[Property]
	public GameObject camParent { get; private set; }

	[Property]
	public Transform camTransform { get; private set; }

	[Property]
	public float WalkingSpeedMultiplier { get; private set; } = 1f;

	[Property]
	public bool WalkingSpeedAffectsFootstepSoundSpeed
	{
		set
		{
			WalkingSpeedAffectsFootstepSoundSpeedScale = (value ? 1 : 0);
		}
	}

	[Property]
	public float WalkingSpeedAffectsFootstepSoundSpeedScale { get; set; }

	[Property]
	public bool ForceStayCrouched { get; set; }

	[Property]
	private bool SnapToNewHeightNextFrame { get; set; }

	[Property]
	public bool ForceCrouched { get; set; }

	[Property]
	public float FieldOfView
	{
		get
		{
			return FieldOfViewBase + FieldOfViewAdditiveModifier;
		}
		set
		{
			FieldOfViewAdditiveModifier = value - FieldOfViewBase;
		}
	}

	[Property]
	public float FieldOfViewBase { get; private set; }

	[Property]
	public float FieldOfViewAdditiveModifier { get; set; }

	[Property]
	public static bool AltKeyPressed
	{
		get
		{
			if ( !Input.Pressed( "Auto-Walk" ) )
			{
				return Input.Pressed( "Auto-Walk" );
			}
			return true;
		}
	}

	private float DeltaTime => Time.Delta;

	public void SetMovementSpeedMultiplier( float newMultiplier )
	{
		WalkingSpeedMultiplier = newMultiplier;
	}

	public void SetCharacterHeightMultiplier( float newMultiplier )
	{
		characterHeightMultipler = newMultiplier;
	}

	public void ResetVelocity()
	{
		movement = (movementGoal = Vector3.Zero);
	}

	private void OnFOVChange( LiveData liveData )
	{
		float fieldOfViewBase = FieldOfViewBase;
		float fieldOfViewAdditiveModifier = FieldOfViewAdditiveModifier;
		float num = fieldOfViewBase + fieldOfViewAdditiveModifier;
		FieldOfViewBase = liveData.FloatValue;
		if ( FieldOfViewAdditiveModifier != 0f )
		{
			FieldOfViewAdditiveModifier = num - FieldOfViewBase;
		}
	}

	protected override void OnAwake()
	{
		if ( _instance == null )
		{
			_instance = this;
		}
		if ( _instance != this )
		{
			GameObject.Destroy();
			return;
		}

		// UnityEngine.Object.DontDestroyOnLoad( base.gameObject );

		if ( this != Instance )
		{
			GameObject.DestroyImmediate();
			return;
		}

		// stanleyTransform = Transform;
		character = Components.Get<CharacterController>();
		cam = Components.GetInChildren<CameraComponent>();
		// camTransform = cam.Transform;
		// camParent = camTransform.Parent;
		// AssetBundleControl.OnScenePreLoad = (Action)Delegate.Combine( AssetBundleControl.OnScenePreLoad, new Action( OnScenePreLoad ) );
		// AssetBundleControl.OnSceneReady = (Action)Delegate.Combine( AssetBundleControl.OnSceneReady, new Action( OnSceneReady ) );
		// AColor = cam.GameObject.Components.Get<AmplifyColorBase>();
		camParentOrigLocalPos = camParent.Transform.LocalPosition;
		CreateFootstepSources();
		CreateFootstepDictionary();
		mainCamera = Components.GetInChildren<MainCamera>();
		FloatConfigurable floatConfigurable = fovSettingConfigurable;
		floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine( floatConfigurable.OnValueChanged, new Action<LiveData>( OnFOVChange ) );
		FieldOfViewBase = fovSettingConfigurable.GetFloatValue();
		gravityMultiplier = groundedGravityMultiplier;
	}

	protected override void OnStart()
	{
		OnSceneReady();
	}

	protected override void OnDestroy()
	{
		FloatConfigurable floatConfigurable = fovSettingConfigurable;
		floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove( floatConfigurable.OnValueChanged, new Action<LiveData>( OnFOVChange ) );
		// AssetBundleControl.OnSceneReady = (Action)Delegate.Remove( AssetBundleControl.OnSceneReady, new Action( OnSceneReady ) );
	}

	private void CreateFootstepDictionary()
	{
		for ( int i = 0; i < footstepCollections.Length; i++ )
		{
			AudioCollection audioCollection = footstepCollections[i];
			int index = audioCollection.GetIndex();
			if ( audioCollection != null && !footstepDictionary.ContainsKey( index ) )
			{
				footstepDictionary.Add( index, audioCollection );
			}
		}
	}

	private void CreateFootstepSources()
	{
		int num = 8;
		/*
		footstepSources = new AudioSource[num];
		for ( int i = 0; i < num; i++ )
		{
			GameObject gameObject = SceneUtility.Instantiate( footstepSource.gameObject );
			footstepSources[i] = gameObject.Components.Get<AudioSource>();
		}
		for ( int j = 0; j < footstepSources.Length; j++ )
		{
			footstepSources[j].transform.parent = footstepSource.transform;
		}
		*/
	}

	private void OnScenePreLoad()
	{
		// _ = (bool)Singleton<GameMaster>.Instance;
		WalkingSpeedMultiplier = 1f;
		WalkingSpeedAffectsFootstepSoundSpeed = false;
		characterHeightMultipler = 1f;
		ResetVelocity();
		FieldOfViewAdditiveModifier = 0f;
		wasCrouching = false;
		ForceCrouched = false;
		ForceStayCrouched = false;
		SnapToNewHeightNextFrame = true;
		if ( cam != null )
		{
			cam.ZFar = 50f;
		}
		cam.GameObject.Transform.LocalPosition = Vector3.Zero;
		cam.GameObject.Transform.LocalRotation = Rotation.Identity;
		FreezeMotionAndView();
	}

	private void OnSceneReady()
	{
		// _ = (bool)Singleton<GameMaster>.Instance;
		masterStartFound = false;
		inAirTimer = 0f;
		outOfBoundsReported = false;
	}

	protected override void OnFixedUpdate()
	{
		velocityAccumulation += character.Velocity.Length * Scene.FixedDelta;
		velocityAccumulation += MathF.Abs( character.Velocity.Length * Scene.FixedDelta ) * footstepsYMultiplier;
	}

	protected override void OnUpdate()
	{
		/*
		if ( !Singleton<GameMaster>.Instance.IsLoading && GameMaster.ONMAINMENUORSETTINGS )
		{
			AudioListener.Volume = Singleton<GameMaster>.Instance.masterVolume;
		}
		*/

		StanleyPosition = Transform.Position;
		cam.FieldOfView = FieldOfViewBase + FieldOfViewAdditiveModifier;

		if ( !viewFrozen )
		{
			View();
		}

		if ( !motionFrozen )
		{
			Movement();
			UpdateCurrentlyStandingOn();
			Footsteps();
			ClickingOnThings();
		}

		else if ( character.Enabled )
		{
			character.Move();
		}

		if ( !viewFrozen )
		{
			FloatCamera();
		}
	}

	private float SensitivityRemap( float n )
	{
		return n * n + 0.5f * n + 0.5f;
	}

	private float HighSensitivityRemap( float n, float factor )
	{
		return n * n + 0.5f * n + 0.5f;
	}

	private float HighSensitivityRemapDoubling( float n, float doubles = 4f )
	{
		float num = 0.5f;
		return MathF.Pow( 2f, n * doubles ) * num;
	}

	private void View()
	{
		// Vector2 vector = Singleton<GameMaster>.Instance.stanleyActions.View.Vector;
		// Vector2 vector2 = new Vector2( Input.GetAxisRaw( "Mouse X" ), Input.GetAxisRaw( "Mouse Y" ) );
		float n = MathX.LerpInverse( controllerSensitivityConfigurable.MinValue, controllerSensitivityConfigurable.MaxValue, controllerSensitivityConfigurable.GetFloatValue() );
		float num = HighSensitivityRemapDoubling( n );
		//vector2.x *= num;
		//vector2.y *= num;
		if ( invertYConfigurable.GetBooleanValue() )
		{
			// vector2.y *= -1f;
		}
		float n2 = MathX.LerpInverse( controllerSensitivityConfigurable.MinValue, controllerSensitivityConfigurable.MaxValue, controllerSensitivityConfigurable.GetFloatValue() );
		float num2 = HighSensitivityRemapDoubling( n2 );
		// vector.x *= num2;
		// vector.y *= num2;
		if ( invertYConfigurable.GetBooleanValue() )
		{
			// vector.y *= -1f;
		}
		if ( Time.Delta > 0f )
		{
			// vector2 /= Time.Delta * 60f;
		}
		/*
		if ( Input.touchCount > 0 )
		{
			vector2 = Vector2.Zero;
		}
		*/
		float num3 = -1f;
		if ( yInvert )
		{
			num3 = 1f;
		}
		// vector2 = new Vector2( vector2.x * mouseSensitivityX, vector2.y * num3 * mouseSensitivityY );
		// vector = new Vector2( vector.x * controllerSensitivityX, vector.y * num3 * controllerSensitivityY );
		Vector2 zero = Vector2.Zero;
		// zero = vector2 + vector * 0.25f;
		zero *= DeltaTime * 70f;
	    Rotation quaternion = Rotation.FromAxis( Vector3.Up, zero.x );
		viewPitch += zero.y;
		viewPitch = MathX.Clamp( viewPitch, -90f, 90f );
		
		/*
		if ( BackgroundCamera.OnRotationUpdate != null )
		{
			BackgroundCamera.OnRotationUpdate( new Vector3( zero.y, zero.x ) );
		}
		*/

		camParent.Transform.LocalRotation = Rotation.FromAxis( Vector3.Right, viewPitch );
		Transform.Rotation = quaternion * Transform.Rotation;
	}

	private bool RayHitGround( Vector3 offset, out GameObject hitGameObject, out int hitTriangleIndex )
	{
		/*
		if (PhysicsTraceBuilder.Ray( Transform.Position + offset, Vector3.Down, out var hitInfo, 3f, groundedLayers ) )
		{
			hitNormal = hitInfo.normal;
			hitGameObject = hitInfo.collider.gameObject;
			hitTriangleIndex = hitInfo.triangleIndex;
			return true;
		}
		*/
		hitGameObject = null;
		hitTriangleIndex = -1;
		return false;
	}

	private void Movement()
	{
		grounded = character.IsOnGround;
		// float y = Singleton<GameMaster>.Instance.stanleyActions.Movement.Y;
		// float x = Singleton<GameMaster>.Instance.stanleyActions.Movement.X;
		// movementInput.x = x;
		// movementInput.z = y;

		/*
		if ( PlatformSettings.Instance.isStandalone.GetBooleanValue() && mouseWalkConfigurable.GetBooleanValue() )
		{
			bool flag = false;
			if ( mouseWalkToggleConfigurable.GetBooleanValue() )
			{
				if ( Singleton<GameMaster>.Instance.stanleyActions.Autowalk.WasPressed )
				{
					autoWalking = !autoWalking;
				}
				flag = autoWalking;
			}
			else
			{
				autoWalking = false;
				
				if ( Singleton<GameMaster>.Instance.stanleyActions.Autowalk.IsPressed )
				{
					flag = true;
				}
			}
			if ( flag )
			{
				movementInput.z = 1f;
			}
		}
		*/

		bool flag2 = Input.Pressed( "Crouch" );

		if ( wasCrouching && ForceStayCrouched )
		{
			flag2 = true;
		}

		if ( ForceCrouched )
		{
			flag2 = true;
		}

		float num = ((!flag2) ? uncrouchedColliderHeight : crouchedColliderHeight);
		character.Height = MathX.Lerp( character.Height, num, crouchSmoothing );

		if ( SnapToNewHeightNextFrame )
		{
			character.Height = num;
			SnapToNewHeightNextFrame = false;
		}

		// cameraParent.LocalPosition = Vector3.Up * character.Height / 2f * characterHeightMultipler;
		camParentOrigLocalPos = camParent.Transform.LocalPosition;
		wasCrouching = flag2;
		movement = movementGoal * walkingSpeed * WalkingSpeedMultiplier;
		// movement = Transform.TransformDirection( movement );
		// BackgroundCamera.OnPositionUpdate?.Invoke( new Vector3( 0f, character.Velocity.y, 0f ) );
		RotatingDoor rotatingDoor = WillHitDoor( movement * Time.Delta );

		if ( rotatingDoor == null )
		{
			if ( lastHitRotatingDoor != null )
			{
				// lastHitRotatingDoor.PlayerTouchingDoor = false;
				lastHitRotatingDoor = null;
			}
		}

		else
		{
			if ( lastHitRotatingDoor != null && lastHitRotatingDoor != rotatingDoor )
			{
				// Debug.LogWarning( "Player is hitting multiple doors this should not happen!\n" + lastHitRotatingDoor.name + "\n" + rotatingDoor.name );
			}
			lastHitRotatingDoor = rotatingDoor;
			// lastHitRotatingDoor.PlayerTouchingDoor = true;
		}

		UpdateInAir( !grounded );

		if ( !grounded )
		{
			gravityMultiplier = MathX.Lerp( gravityMultiplier, 1f, Time.Delta * gravityFallAcceleration );
			movement *= inAirMovementMultiplier;
		}

		else
		{
			gravityMultiplier = groundedGravityMultiplier;
		}

		if ( flag2 )
		{
			movement *= crouchMovementMultiplier;
		}

		if ( character.Enabled )
		{
			// character.Move((movement + Vector3.Up * maxGravity * gravityMultiplier) * Time.Delta );
		}
	}

	private void UpdateInAir( bool inAir )
	{
		if ( !outOfBoundsReported && inAir && character.Velocity.y <= -1f /* && !GameMaster.PAUSEMENUACTIVE && !Singleton<GameMaster>.Instance.IsLoading */ )
		{
			inAirTimer += Time.Delta;
			if ( inAirTimer >= inAirLimit )
			{
				if ( OnOutOfBounds != null )
				{
					OnOutOfBounds();
				}
				outOfBoundsReported = true;
			}
		}
		else
		{
			inAirTimer = 0f;
		}
	}

	private RotatingDoor WillHitDoor( Vector3 motion )
	{
		// Vector3 vector = character.Transform.Position + motion + character.Center;
		// Collider[] array = Physics.OverlapCapsule( vector - Vector3.Up * character.Height / 2f, vector + Vector3.Up * character.Height / 2f, character.Radius );
		/*
		foreach ( Collider collider in array )
		{
			RotatingDoor component = collider.GameObject.Components.Get<RotatingDoor>();
			if ( !(component != null) )
			{
				continue;
			}
			// Vector3 vector2 = collider.ClosestPoint( character.Transform.Position );
			Vector3 vector3 = character.Transform.Position - vector2;
			float magnitude = vector3.Length;
			if ( magnitude < character.Radius )
			{
				if ( magnitude == 0f )
				{
					// Debug.LogError( "[StanleyController] Distance to door should NEVER be zero, check if door collider is MeshCollider, replace it with a Box Collider please" );
				}
				else
				{
					Vector3 vector4 = vector3 / magnitude;
					float num = character.Radius - magnitude;
					// character.Move( vector4 * num );
				}
			}
			return component;
		}
		*/
		return null;
	}

	/*
	private void OnControllerColliderHit( ControllerColliderHit hit )
	{
		hitNormal = hit.normal;
	}
	*/

	private void ClickingOnThings()
	{
		/*
		if ( Singleton<GameMaster>.Instance.FullScreenMoviePlaying || !Singleton<GameMaster>.Instance.stanleyActions.UseAction.WasPressed )
		{
			return;
		}
		*/

		/*
		if ( PhysicsTraceBuilder.Ray( camParent.Transform.Position, camParent.Forward, out var hitInfo, armReach, clickLayers, QueryTriggerInteraction.Ignore ) )
		{
			GameObject gameObject = hitInfo.collider.gameObject;
			HammerEntity component = gameObject.Components.Get<HammerEntity>();
			if ( component != null )
			{
				component.Use();
			}
			else
			{
				PlayKeyboardSound();
			}
			if ( OnInteract != null )
			{
				OnInteract( gameObject );
			}
		}

		else
		{
			PlayKeyboardSound();
			if ( OnInteract != null )
			{
				OnInteract( null );
			}
		}
		*/
	}

	private void OnApplicationPause( bool pause )
	{
		if ( pause && character.Enabled )
		{
			character.Accelerate( Vector2.Zero );
		}
	}

	/*
	private void PlayFootstepSound( AudioEntry entry )
	{
		AudioClip clip;
		bool clip2 = entry.GetClip( out clip );
		AudioSource availableFootstepSource = GetAvailableFootstepSource();
		if ( clip2 && availableFootstepSource != null )
		{
			availableFootstepSource.clip = clip;
			availableFootstepSource.pitch = entry.GetPitch();
			availableFootstepSource.volume = entry.GetVolume();
			availableFootstepSource.Play();
		}
	}
	*/

	/*
	private AudioSource GetAvailableFootstepSource()
	{
		for ( int i = 0; i < footstepSources.Length; i++ )
		{
			AudioSource audioSource = footstepSources[i];
			if ( !audioSource.isPlaying )
			{
				return audioSource;
			}
		}
		return null;
	}
	*/

	private void PlayKeyboardSound()
	{
		// useSource.Stop();
		// (Singleton<GameMaster>.Instance.barking ? barkingCollection : keyboardCollection).SetVolumeAndPitchAndPlayClip( useSource );
	}

	/*
	public void Teleport( TeleportType style, Vector3 destination, Vector3 up, bool useAngle, bool freezeAtStartOfTeleport = true, bool unfreezeAtEndOfTeleport = true, Transform orientationTransform = null )
	{
		Teleport( style, base.Transform.Position, destination, up, useAngle, freezeAtStartOfTeleport, unfreezeAtEndOfTeleport, orientationTransform );
	}
	*/

	/*
	public void Teleport( TeleportType style, Vector3 landmark, Vector3 destination, Vector3 up, bool useAngle, bool freezeAtStartOfTeleport = true, bool unfreezeAtEndOfTeleport = true, Transform orientationTransform = null )
	{
		bool flag = true;
		if ( freezeAtStartOfTeleport )
		{
			FreezeMotionAndView();
		}
		switch ( style )
		{
			case TeleportType.PlayerStart:
				flag = !masterStartFound;
				break;
			case TeleportType.PlayerStartMaster:
				masterStartFound = true;
				break;
			case TeleportType.TriggerTeleport:
				flag = true;
				break;
		}
		if ( flag )
		{
			Vector3 vector = base.Transform.Position - landmark;
			if ( PhysicsTraceBuilder.Ray( new Ray( destination, Vector3.Down ), out var hitInfo, 2f, groundedLayers, QueryTriggerInteraction.Ignore ) )
			{
				Vector3 vector2 = destination + vector;
				float num = Instance.character.Height / 2f + Instance.character.skinWidth;
				base.Transform.Position = new Vector3( vector2.x, hitInfo.point.y + num, vector2.z );
			}
			else
			{
				base.Transform.Position = destination + vector + Vector3.Up * 0.05f;
			}
			velocityAccumulation = 0f;
			if ( useAngle && orientationTransform != null )
			{
				Transform.Rotation = orientationTransform.Rotation;
				Transform.Rotate( 90f, 0f, 0f, Space.Self );
				float num2 = Vector3.Angle( base.Transform.Up, Vector3.Up );
				Transform.Rotate( 0f - num2, 0f, 0f, Space.Self );
				float value = Vector3.Angle( Transform.Forward, -orientationTransform.Up );
				viewPitch = MathX.Clamp( value, -90f, 90f );
				camParent.Transform.LocalRotation = Rotation.FromAxis( Vector3.Right, viewPitch );
			}
			else if ( useAngle )
			{
				Vector3 rhs = Vector3.Cross( Vector3.Up, up );
				Vector3 vector3 = Vector3.Cross( up, rhs );
				Vector3 vector4 = new Vector3( up.x, 0f, up.z );
				if ( vector4 != Vector3.Zero )
				{
					base.Transform.Rotation = Rotation.LookAt( vector4, Vector3.Up );
				}
				Vector3 vector5 = new Vector3( 0f, vector3.y, 0f );
				if ( vector5 != Vector3.Zero )
				{
					viewPitch = Vector3.Angle( Vector3.Up, vector5 );
					camParent.Transform.LocalRotation = Rotation.FromAxis( Vector3.Right, viewPitch );
					
					if ( BackgroundCamera.OnAlignToTransform != null )
					{
						BackgroundCamera.OnAlignToTransform( camParent );
					}
				}
			}
			if ( (style == TeleportType.PlayerStart || style == TeleportType.PlayerStartMaster) && OnTeleportToPlayerStart != null )
			{
				OnTeleportToPlayerStart();
			}
		}
		if ( unfreezeAtEndOfTeleport )
		{
			UnfreezeMotionAndView();
		}
	}
	*/
	

	public void Deparent( bool kill = false )
	{
		GameObject gameObject = null;
		if ( Transform.Parent != null )
		{
			gameObject = Transform.Parent;
		}

		GameObject.SetParent( null );

		if ( kill && gameObject != null )
		{
			GameObject.Destroy();
		}

		// UnityEngine.Object.DontDestroyOnLoad( base.gameObject );
	}

	public void ParentTo( GameObject adopter )
	{
		GameObject.SetParent( adopter );
	}

	public void EnableCamera()
	{
		cam.Enabled = true;
		currentCam = cam;
	}

	public void DisableCamera( CameraComponent replacement )
	{
		cam.Enabled = false;
		currentCam = replacement;
	}

	public void FreezeMotion( bool commandFreezeMotion = false )
	{
		motionFrozen = true;
		character.Enabled = false;
		if ( commandFreezeMotion )
		{
			frozenFromCommandMotion = true;
		}
	}

	public void UnfreezeMotion( bool commandUnfreezeMotion = false )
	{
		if ( commandUnfreezeMotion || !frozenFromCommandMotion )
		{
			motionFrozen = false;
			character.Enabled = true;
			if ( commandUnfreezeMotion && frozenFromCommandMotion )
			{
				frozenFromCommandMotion = false;
			}
		}
	}

	public void FreezeView( bool commandFreezeView = false )
	{
		viewFrozen = true;
		if ( commandFreezeView )
		{
			frozenFromCommandView = true;
		}
	}

	public void UnfreezeView( bool commandUnfreezeView = false )
	{
		if ( commandUnfreezeView || !frozenFromCommandView )
		{
			viewFrozen = false;
			if ( commandUnfreezeView )
			{
				frozenFromCommandView = false;
			}
		}
	}

	public void ResetClientCommandFreezes()
	{
		frozenFromCommandView = false;
		frozenFromCommandMotion = false;
	}

	public void FreezeMotionAndView()
	{
		FreezeMotion();
		FreezeView();
	}

	public void UnfreezeMotionAndView()
	{
		UnfreezeMotion();
		UnfreezeView();
	}

	public void StartFloating()
	{
		floatingPos = 0f;
		// StartCoroutine( FloatFadeInOut( 1f ) );
	}

	public void EndFloating()
	{
		// StartCoroutine( FloatFadeInOut() );
	}

	private IEnumerator FloatFadeInOut( float targetStrength = 0f )
	{
		while ( floatingStrength != targetStrength )
		{
			floatingStrength = MathX.Lerp( floatingStrength, targetStrength, Time.Delta / 2f );
			yield return null;
		}
	}

	private void FloatCamera()
	{
		float num = MathF.Pow( MathF.Sin( floatingPos ), 2f ) * 0.33f;
		// camParent.LocalPosition = camParentOrigLocalPos + Vector3.Up * num * floatingStrength;
		floatingPos += Time.Delta / 3f;
	}

	private void ResetFloat()
	{
		floatingStrength = 0f;
		// StopCoroutine( "FloatFadeInOut" );
		// camParent.LocalPosition = camParentOrigLocalPos;
	}

	public void NewMapReset()
	{
		ResetFloat();
		EnableCamera();
		// camParent.LocalRotation = Rotation.Identity;
		masterStartFound = false;
		if ( mainCamera != null )
		{
			// mainCamera.UpdatePortals();
		}
	}

	private void UpdateCurrentlyStandingOn()
	{
		standingOnUpdateTimer += Time.Delta;
		if ( standingOnUpdateTimer >= standingOnUpdateLimit )
		{
			FindFootstepMaterial();
			standingOnUpdateTimer = 0f;
		}
	}

	private void Footsteps()
	{
		float num = ((currentFootstepAudioCollection != null) ? currentFootstepAudioCollection.AverageDuration : 0.69f);
		float num2 = num;
		_ = WalkingSpeedMultiplier;
		num2 = MathX.Lerp( num * WalkingSpeedMultiplier, num, WalkingSpeedAffectsFootstepSoundSpeedScale );
		if ( !(velocityAccumulation >= num2) )
		{
			return;
		}
		velocityAccumulation = 0f;
		int key = (int)currentlyStandingOn;
		if ( footstepDictionary.TryGetValue( key, out currentFootstepAudioCollection ) )
		{
			/*
			AudioSource availableFootstepSource = GetAvailableFootstepSource();
			if ( availableFootstepSource != null )
			{
				currentFootstepAudioCollection.SetVolumeAndPitchAndPlayClip( availableFootstepSource );
			}
			*/
		}
	}

	private void FindFootstepMaterial()
	{
		StanleyData.FootstepSounds footstepSounds = currentlyStandingOn;
		if ( RayHitGround( Vector3.Up * 0.1f, out var hitGameObject, out var hitTriangleIndex ) )
		{
			ModelRenderer component = hitGameObject.Components.Get<ModelRenderer>();
			NonRendererFootstepType component2 = hitGameObject.Components.Get<NonRendererFootstepType>();
			if ( component2 != null && (!(component != null) || !component2.ForceOverrideMaterial) )
			{
				currentlyStandingOn = component2.FootstepType;
				if ( playFootstepOnNewMaterial && footstepSounds != currentlyStandingOn )
				{
					velocityAccumulation = 99f;
				}
				return;
			}
			if ( component != null )
			{
				ModelCollider component3 = hitGameObject.Components.Get<ModelCollider>();
				/*
				if ( component3 != null && component.sharedMaterials.Length >= 2 && GetMaterialFromTriangleIndex( component, component3, hitTriangleIndex, out var foundMaterial ) )
				{
					if ( foundMaterial.HasProperty( "_FootstepType" ) )
					{
						currentlyStandingOn = (StanleyData.FootstepSounds)foundMaterial.GetFloat( "_FootstepType" );
						if ( playFootstepOnNewMaterial && footstepSounds != currentlyStandingOn )
						{
							velocityAccumulation = 99f;
						}
						return;
					}
				}
				else
				{
					for ( int i = 0; i < component.sharedMaterials.Length; i++ )
					{
						Material material = component.sharedMaterials[i];
						if ( material != null && material.HasProperty( "_FootstepType" ) )
						{
							currentlyStandingOn = (StanleyData.FootstepSounds)material.GetFloat( "_FootstepType" );
							if ( playFootstepOnNewMaterial && footstepSounds != currentlyStandingOn )
							{
								velocityAccumulation = 99f;
							}
							return;
						}
					}
				}
				*/
			}
		}
		currentlyStandingOn = StanleyData.FootstepSounds.Silence;
	}

	private bool GetMaterialFromTriangleIndex( ModelRenderer mRenderer, ModelCollider mCollider, int triIndex, out Material foundMaterial )
	{
		/*
		if ( !(mCollider.sharedMesh == null) )
		{
			Mesh sharedMesh = mCollider.sharedMesh;
			if ( sharedMesh.isReadable && sharedMesh.triangles.Length >= triIndex * 3 + 3 )
			{
				if ( triIndex < 0 )
				{
					_ = "triangle index is less than zero " + sharedMesh.name + " of " + mRenderer.name + " index = " + triIndex;
				}
				else
				{
					int num = sharedMesh.triangles[triIndex * 3];
					int num2 = sharedMesh.triangles[triIndex * 3 + 1];
					int num3 = sharedMesh.triangles[triIndex * 3 + 2];
					int num4 = -1;
					for ( int i = 0; i < sharedMesh.subMeshCount; i++ )
					{
						int[] triangles = sharedMesh.GetTriangles( i );
						for ( int j = 0; j < triangles.Length; j += 3 )
						{
							if ( triangles[j] == num && triangles[j + 1] == num2 && triangles[j + 2] == num3 )
							{
								num4 = i;
								break;
							}
						}
						if ( num4 != -1 )
						{
							break;
						}
					}
					if ( num4 != -1 )
					{
						foundMaterial = mRenderer.sharedMaterials[num4];
						return foundMaterial != null;
					}
				}
			}
		}
		*/
		foundMaterial = null;
		return false;
	}

	public void StartPostProcessFade( Texture lut, float startVal, float endVal, float duration )
	{
		// AColor.LutBlendTexture = lut;
		// StartCoroutine( PostProcessFade( startVal, endVal, duration ) );
	}

	private IEnumerator PostProcessFade( float start, float end, float duration )
	{
		float startTime = Time.Delta;
		float endTime = startTime + duration;
		while ( Time.Delta < endTime )
		{
			float t = MathX.LerpInverse( startTime, endTime, Time.Delta );
			// AColor.BlendAmount = MathX.Lerp( start, end, t );
			yield return null;
		}
		/*
		AColor.BlendAmount = end;
		if ( end == 0f )
		{
			AColor.LutBlendTexture = null;
		}
		*/
	}

	public void SetFarZ( float value, FarZVolume.CameraModes cameraMode = FarZVolume.CameraModes.RenderSkybox )
	{
		if ( FarZCoroutine != null )
		{
			// StopCoroutine( FarZCoroutine );
		}
		FarZCoroutine = FarZ( value, cameraMode );
		// StartCoroutine( FarZCoroutine );
	}

	private IEnumerator FarZ( float value, FarZVolume.CameraModes cameraMode = FarZVolume.CameraModes.RenderSkybox )
	{
		float startTime = Time.Delta;
		float endTime = startTime + 1f;
		float startZ = cam.ZFar;

		switch ( cameraMode )
		{
			case FarZVolume.CameraModes.RenderSkybox:
				// cam.ClearFlags = CameraClearFlags.Skybox;
				break;
			case FarZVolume.CameraModes.DepthOnly:
				// cam.ClearFlags = CameraClearFlags.Depth;
				break;
		}

		while ( Time.Delta < endTime )
		{
			float t = MathX.LerpInverse( startTime, endTime, Time.Delta );
			cam.ZFar = MathX.Lerp( startZ, value, t );
			yield return null;
		}

		cam.ZFar = value;
	}

	private void LogController()
	{
		/*
		if ( !(Singleton<GameMaster>.Instance.stanleyActions.Movement.Vector != Vector2.Zero) )
		{
			_ = Singleton<GameMaster>.Instance.stanleyActions.View.Vector != Vector2.Zero;
		}
		_ = Singleton<GameMaster>.Instance.stanleyActions.UseAction.WasPressed;
		_ = Singleton<GameMaster>.Instance.stanleyActions.JumpAction.WasPressed;
		*/
	}
}
