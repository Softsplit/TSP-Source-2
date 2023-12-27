using Sandbox;
using System;

[AttributeUsage(AttributeTargets.Property)]
public class InspectorButtonAttribute : PropertyAttribute
{
    public float ButtonWidth
    {
        get
        {
            return _buttonWidth;
        }

        set
        {
            _buttonWidth = value;
        }
    }

    public InspectorButtonAttribute(string MethodName, string DisplayName = null)
    {
        this.MethodName = MethodName;
        this.DisplayName = DisplayName ?? MethodName;
    }

    public static float kDefaultButtonWidth;

    public readonly string MethodName;

    public readonly string DisplayName;

    private float _buttonWidth = kDefaultButtonWidth;
}