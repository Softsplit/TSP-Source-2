using Editor;
using Sandbox;

/// <summary>
/// A message forwarder. Fires an OnTrigger output when triggered, and can be disabled to prevent forwarding outputs.<br/>
/// <br/>
/// Useful as an intermediary between one entity and another for turning on or off an I/O connection, or as a container for holding a set of outputs that can be triggered from multiple places.
/// </summary>
[Library("logic_relay")]
[HammerEntity]
[VisGroup(VisGroup.Logic)]
[EditorSprite("editor/logic_relay.vmat")]
[Title("Relay"), Category("Logic"), Icon("east")]
public partial class LogicRelay : Entity
{
    [Property(Title = "Start Disabled")]
    public bool StartDisabled { get; set; } = false;

    /// <summary>
    /// Enable this entity.
    /// </summary>
    [Input]
    public void Enable()
    {
        // WIP
    }

    /// <summary>
    /// Disable this entity.
    /// </summary>
    [Input]
    public void Disable()
    {
        // WIP
    }

    /// <summary>
    /// Trigger the relay, causing its OnTrigger output to fire if it is enabled.
    /// </summary>
    [Input]
    public void Trigger()
    {
        // WIP
    }

    /// <summary>
    /// Toggle the relay between enabled and disabled.
    /// </summary>
    [Input]
    public void Toggle()
    {
        // WIP
    }

    /// <summary>
    /// Cancel any events fired by this relay that are currently pending in the I/O event queue.
    /// </summary>
    [Input]
    public void CancelPending()
    {
        // WIP
    }

    /// <summary>
    /// Fired when the relay is spawned. If the relay is set to only trigger once, it will delete itself after firing this output.
    /// </summary>
    protected Output OnSpawn { get; set; }

    /// <summary>
    /// Fired when the relay is triggered. If the relay is set to only trigger once, it will delete itself after firing this output.
    /// </summary>
    protected Output OnTrigger { get; set; }

    public override void Spawn()
    {
        base.Spawn();

        // WIP
    }
}

