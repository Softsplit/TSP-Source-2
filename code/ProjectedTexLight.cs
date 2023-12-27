using Sandbox;

public sealed class ProjectedTexLight : HammerEntity
{
    protected override void OnAwake()
    {
        _light = Components.Get<SpotLight>();

        if (!StartActive)
        {
            if (_light != null)
            {
                _light.Enabled = false;
            }

            if (Connected != null)
            {
                Connected.Enabled = false;
            }
        }
    }

    public void Input_TurnOn()
    {
        if (_light != null)
        {
            _light.Enabled = true;
        }

        if (Connected != null)
        {
            Connected.Enabled = true;
        }
    }

    public void Input_TurnOff()
    {
        if (_light != null)
        {
            _light.Enabled = false;
        }

        if (Connected != null)
        {
            Connected.Enabled = false;
        }
    }

    private SpotLight _light;

    [Property]
    public bool StartActive { get; set; } = true;

    [Property]
    public GameObject Connected { get; set; }
}