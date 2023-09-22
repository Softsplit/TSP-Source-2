using Sandbox;
using Editor;

/// <summary>
/// A door which is linked by a portal to another 'linked_portal_door' entity.
/// </summary>
[Library("linked_portal_door"), HammerEntity]
[EditorModel("models/editor/axis_helper_thick.vmdl")]
[Title("Linked Portal Door"), Category("Miscellaneous"), Icon("door_front")]
public partial class LinkedPortalDoor : Entity
{
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

    /// <summary>
    /// Whether to start the linkage as active from the start.
    /// </summary>
    [Property(Title = "Start Active")]
    public bool StartActive { get; set; } = false;

    /// <summary>
    /// Another 'prop_linked_portal_door' entity which will link to this one.
    /// </summary>
    [Property(Title = "Linked Partner")]
    public EntityTarget PartnerName { get; set; }

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
        // WIP
    }

    /// <summary>
    /// Close the door and cause the portal to deactivate.
    /// </summary>
    [Input]
    public void Close()
    {
        // WIP
    }

    /// <summary>
    /// Called when the door has started its open animation.
    /// </summary>
    public Output OnOpen { get; set; }
    /// <summary>
    /// Called when the door has started its close animation.
    /// </summary>
    public Output OnClose { get; set; }
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

    public override void Spawn()
    {
        base.Spawn();

        // WIP
    }
}