using System;
using System.ComponentModel;
using System.Linq;
using Editor;
using Sandbox;
using Sandbox.Internal;

// This is a reconstructed and modified version of https://asset.party/alex/world_portals with inputs and outputs,
// which itself is also borrowed from https://youtu.be/cWpFZbjtSQg?si=05_qrpo9v1GMSpdV. Borrowception!

/// <summary>
/// A door which is linked by a portal to another 'linked_portal_door' entity.
/// </summary>
[Library("linked_portal_door"), HammerEntity]
[EditorModel("models/editor/axis_helper_thick.vmdl")]
[Title("Linked Portal Door"), Category("Miscellaneous"), Icon("door_front")]
public partial class LinkedPortalDoor : RenderEntity
{
    private Material material = Material.Load("materials/linked_portal_door.vmat");
    private Texture colorTarget;
    private Texture depthTarget;
    private static TimeSince TimeSinceTeleported = 0f;
    public static float RenderScale = 1f;

    /// <summary>
    /// Width of the desired portal.
    /// </summary>
    [Property(Title = "Width")]
    public int Width { get; set; } = 128;

    /// <summary>
    /// Height of the desired portal.
    /// </summary>
    [Property(Title = "Height")]
    public int Height { get; set; } = 128;

    [Description("BBox for this portal")]
    private BBox LocalBBox => new BBox(new Vector3(-4f, 0f - Width, 0f - Height), new Vector3(4f, Width, Height));
    private BBox WorldBBox => new BBox(Position + Rotation * LocalBBox.Mins, Position + Rotation * LocalBBox.Maxs);

    /// <summary>
    /// Whether to start the linkage as active from the start.
    /// </summary>
    [Property(Title = "Start Active")]
    public static bool StartActive { get; set; } = false;

    /// <summary>
    /// Another 'prop_linked_portal_door' entity which will link to this one.
    /// </summary>
    [Property(Title = "Linked Partner")]
    public EntityTarget PartnerName { get; set; }

    [Net]
    [Description("The linked partner")]
    public LinkedPortalDoor Partner { get; set; }

    /// <summary>
    /// Called when the door has started its open animation.
    /// </summary>
    public Output OnOpen { get; set; }
    /// <summary>
    /// Called when the door has started its close animation.
    /// </summary>
    public Output OnClose { get; set; }

    /*
    /// <summary>
    /// When any entity is teleported from this portal to the linked partner.
    /// </summary>
    public Output OnEntityTeleportFromMe { get; set; }
    /// <summary>
    /// When the player is teleported from this portal to the linked partner.
    /// </summary>
    public Output OnPlayerTeleportFromMe { get; set; }
    /// <summary>
    /// When any entity is teleported from this linked partner to the portal.
    /// </summary>
    public Output OnEntityTeleportToMe { get; set; }
    /// <summary>
    /// When the player is teleported from this linked partner to the portal.
    /// </summary>
    public Output OnPlayerTeleportToMe { get; set; }
    */

    /// <summary>
    /// Set a new partner door.
    /// </summary>
    [Input]
    public void SetPartner(string partnerName)
    {
        PartnerName = partnerName;
    }

    /// <summary>
    /// Open the door and cause the portal to activate.
    /// </summary>
    [Input]
    public void Open()
    {
        if (StartActive != true)
        {
            StartActive = true;
        }

        OnOpen.Fire(this);
    }

    /// <summary>
    /// Close the door and cause the portal to deactivate.
    /// </summary>
    [Input]
    public void Close()
    {
        if (StartActive != false)
        {
            StartActive = false;
        }

        OnClose.Fire(this);
    }

    public override void Spawn()
    {        
        base.Transmit = TransmitType.Always;
    }

    [GameEvent.Tick]
    public void OnTick()
    {       
        Partner = PartnerName.GetTarget<LinkedPortalDoor>();

        if (!Game.IsServer || (float)TimeSinceTeleported < 0.1f)
        {
            return;
        }

        foreach (Entity entity in Entity.FindInBox(WorldBBox))
        {           
            RenderBounds = new BBox(LocalBBox.Mins * Rotation, LocalBBox.Maxs * Rotation);
            
            Vector3 eyePos = entity.Position + Vector3.Up * 64f;

            if (WorldBBox.Contains(eyePos))
            {
                if (entity.Velocity.Length < 50f)
                {
                    break;
                }

                Transform entityLocal = base.Transform.ToLocal(entity.Transform);
                
                entity.Position = Partner.Transform.PointToWorld(entityLocal.Position);                
                entity.Position += Vector3.Up;               
                entity.AngularVelocity = new Angles(0f, 90f, 0f);

                Rotation.LookAt((Partner.Position - Position).Normal, Vector3.Up);
                
                TimeSinceTeleported = 0f;
            }
        }
    }

    private void ProtectFromClipping(out Vector3 scale, out Vector3 offset)
    {
        float halfHeight = Camera.ZNear * MathF.Tan((Camera.FieldOfView * 0.5f).DegreeToRadian());
        float dstToNearClipPlaneCorner = new Vector3(halfHeight * Screen.Aspect, halfHeight, Camera.ZNear).Length * 0.0254f;
        float dot = Vector3.Dot(Rotation.Forward, (Position - Camera.Position).Normal);
        
        scale = new Vector3(1f, 1f, 1f) * Rotation;        
        offset = Vector3.Forward * (dstToNearClipPlaneCorner * ((dot > 0f) ? 1f : (-1f)) * 20f);
    }

    public override void UpdateSceneObject(SceneObject obj)
    {       
        base.UpdateSceneObject(obj);
        
        obj.Flags.IsTranslucent = false;        
        obj.Flags.IsOpaque = true;        
        obj.Flags.BloomLayer = false;
        obj.Flags.CastShadows = false;       
        obj.Flags.OverlayLayer = false;      
        obj.RenderLayer = SceneRenderLayer.Default;
    }

    [GameEvent.Client.Frame]
    public static void RenderAllPortals()
    {
        foreach (LinkedPortalDoor portal in Entity.All.OfType<LinkedPortalDoor>())
        {           
            StartActive = true;
            
            portal.colorTarget = Texture.CreateRenderTarget("LinkedPortalDoorTexture:Color", ImageFormat.RGB565, Screen.Size * RenderScale, portal.colorTarget);           
            portal.depthTarget = Texture.CreateRenderTarget("LinkedPortalDoorTexture:Depth", ImageFormat.D24S8, Screen.Size * RenderScale, portal.depthTarget);    
            
            portal.RenderScene(RenderTarget.From(portal.colorTarget, portal.depthTarget));
            
            StartActive = false;
        }
    }

    private void RenderScene(RenderTarget renderTarget)
	{
		SceneCamera renderCamera;
		if (Game.IsClient && Partner.IsValid())
		{
			Transform cameraTransform = new Transform(Camera.Position, Camera.Rotation);
			Transform localCamera = base.Transform.ToLocal(cameraTransform);
			Transform endCamera = Partner.Transform.ToWorld(localCamera);

			renderCamera = new SceneCamera("LinkedPortalDoorCamera");
			renderCamera.World = Game.SceneWorld;
			renderCamera.Position = endCamera.Position;
			renderCamera.Rotation = endCamera.Rotation;
			renderCamera.FieldOfView = Camera.FieldOfView;
			renderCamera.ZNear = 1f;
			renderCamera.ZFar = 2048f;
			renderCamera.BackgroundColor = Color.Black;
			renderCamera.EnablePostProcessing = true;

			float distance = (Position - Camera.Position).Length;
			Plane clipPlane2 = new Plane(endCamera.Rotation.Forward, (double)distance - 256.0);
			
			SetClipPlane(clipPlane2);
			
			Graphics.RenderToTexture(renderCamera, renderTarget.ColorTarget);
		}
		void SetClipPlane(Plane clipPlane)
		{
			RenderAttributes attributes = renderCamera.Attributes;

			string i = "EnableClipPlane";
			bool value = true;
			
			attributes.Set(in i, in value);
			RenderAttributes attributes2 = renderCamera.Attributes;
			
			i = "ClipPlane0";
			Vector4 value2 = new Vector4(in clipPlane.Normal, clipPlane.Distance);
			
			attributes2.Set(in i, in value2);
		}
	}

    public override void DoRender(SceneObject obj)
	{
		if (!StartActive)
		{
			_ = (Position - Partner.Position).Normal;
			
			RenderAttributes attributes = Graphics.Attributes;

			string k = "ColorTarget";
			int mip = -1;

			attributes.Set(in k, in colorTarget, in mip);
			
			Graphics.GrabDepthTexture("DepthTarget");
			
			ProtectFromClipping(out var scale, out var offset);

			VertexBuffer vertexBuffer = new VertexBuffer();
			
			vertexBuffer.Init(useIndexBuffer: true);			
			vertexBuffer.AddCube(Vector3.Zero + offset, LocalBBox.Size * scale, Rotation.Identity);			
			vertexBuffer.Draw(material);
		}
	}
}
