using Editor;
using Sandbox;

/// <summary>
/// Compares an input value to another value. If the input value is less than the compare value, the OnLessThan output is fired with the input value. If the input value is equal to the compare value, the OnEqualTo output is fired with the input value. If the input value is greater than the compare value, the OnGreaterThan output is fired with the input value.
/// </summary>
[Library("logic_compare")]
[HammerEntity]
[VisGroup(VisGroup.Logic)]
[EditorSprite("editor/logic_compare.vmat")]
[Title("Compare"), Category("Logic"), Icon("compare_arrows")]
public partial class LogicCompare : Entity
{
    /// <summary>
    /// Initial value for the input value.
    /// </summary>
    [Property(Title = "Initial value")]
    public int InitialValue { get; set; } = 0;

    /// <summary>
    /// The value to compare against.
    /// </summary>
    [Property(Title = "Compare value")]
    public int CompareValue { get; set; } = 0;

    /// <summary>
    /// Set the value that will be compared against the compare value.
    /// </summary>
    [Input]
    public void SetValue(float initialValue)
    {
        // WIP
    }

    /// <summary>
    /// Set the value that will be compared against the compare value and performs the comparison.
    /// </summary>
    [Input]
    public void SetValueCompare(float toCompareValue)
    {
        // WIP
    }

    /// <summary>
    /// Set the compare value.
    /// </summary>
    [Input]
    public void SetCompareValue(float compareValue)
    {
        // WIP
    }

    /// <summary>
    /// Force a compare of the input value with the compare value.
    /// </summary>
    [Input]
    public void Compare()
    {
        // WIP
    }

    /// <summary>
    /// Fired when the input value is less than the compare value. Sends the input value as data.
    /// </summary>
    protected Output OnLessThan { get; set; }

    /// <summary>
    /// Fired when the input value is equal to the compare value. Sends the input value as data.
    /// </summary>
    protected Output OnEqualTo { get; set; }

    /// <summary>
    /// Fired when the input value is different from the compare value. Sends the input value as data.
    /// </summary>
    protected Output OnNotEqualTo { get; set; }

    /// <summary>
    /// Fired when the input value is greater than the compare value. Sends the input value as data.
    /// </summary>
    protected Output OnGreaterThan { get; set; }

    public override void Spawn()
    {
        base.Spawn();

        // WIP
    }
}

