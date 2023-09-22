using Editor;
using Sandbox;


/// <summary>
/// An entity that fires a timer event at regular, or random, intervals. It can also be set to oscillate betweena high and low end, in which case it will fire alternating high/low outputs each time it fires.
/// </summary>
[Library("logic_timer"), HammerEntity]
[EditorSprite("editor/logic_timer.vmat"), VisGroup(VisGroup.Logic)]
[Title("Timer"), Category("Logic"), Icon("timer")]
public partial class LogicTimer : Entity
{
    [Property(Title = "Start Disabled")]
    public bool StartDisabled { get; set; } = false;

    [Property(Title = "Use Random Time")]
    public bool UseRandomTime { get; set; } = false;

    /// <summary>
    /// If 'Use Random Time' is set, this is the minimum time between timer fires. The time will be a random number between this and the 'Maximum Random Interval'.
    /// </summary>
    [Property(Title = "Minimum Random Interval")]
    public float LowerRandomBound { get; set; }

    /// <summary>
    /// If 'Use Random Time' is set, this is the maximum time between timer fires. The time will be a random number between the 'Minimum Random Interval' and this.
    /// </summary>
    [Property(Title = "Maximum Random Interval")]
    public float UpperRandomBound { get; set; }

    /// <summary>
    /// If 'Use Random Time' isn't set, this is the time between timer fires, in seconds.
    /// </summary>
    [Property("RefireTime", Title = "Refire Interval")]
    public float RefireInterval { get; set; }

    /// <summary>
    /// Enable the timer.
    /// </summary>
    [Input]
    public void Enable()
    {
        // WIP
    }

    /// <summary>
    /// Disable the timer.
    /// </summary>
    [Input]
    public void Disable()
    {
        // WIP
    }

    /// <summary>
    /// Toggle the timer on/off.
    /// </summary>
    [Input]
    public void Toggle()
    {
        // WIP
    }

    /// <summary>
    /// Set a new Refire Interval.
    /// </summary>
    [Input]
    public void RefireTime(int refireTime)
    {
        // WIP
    }

}


