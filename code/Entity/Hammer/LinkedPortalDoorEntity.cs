using Sandbox;
using Editor;

[Library("linked_portal_door"), HammerEntity]
[Title("Linked Portal Door"), Category("Portal"), Icon("editor/portal.png")]
public partial class LinkedPortalDoorEntity : Entity
{
    [Property(Title = "Width")]
    public int Width { get; set; } = 256;

    [Property(Title = "Height")]
    public int Height { get; set; } = 128;

    [Property(Title = "Start Active")]
    public bool StartActive { get; set; } = true;

    [Property(Title = "Linked Partner")]
    public string LinkedPartner { get; set; } = "";

    [Input]
    public void SetPartner(string partnerName)
    {
        LinkedPartner = partnerName;
    }

    [Input]
    public void Open()
    {
        // WIP
    }

    [Input]
    public void Close()
    {
        // WIP
    }

    public Output OnOpen { get; set; }
    public Output OnClose { get; set; }
    public Output OnEntityTeleportFromMe { get; set; }
    public Output OnPlayerTeleportFromMe { get; set; }
    public Output OnEntityTeleportToMe { get; set; }
    public Output OnPlayerTeleportToMe { get; set; }

    public override void Spawn()
    {
        base.Spawn();

        // Activate the portal if StartActive is true
        if (StartActive)
        {
            Open();
        }
    }
}