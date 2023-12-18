using Sandbox;

public sealed class ImportedObject : HammerEntity
{
    [Property] public string classname { get; set; } = "";

    [Property] public bool entity { get; set; } = true;

    [Property] public bool brush { get; set; }

    [Property] public int id { get; set; } = -1;

    [Property] public bool modified { get; set; }

    [Property] public bool dontKillChildren { get; set; } = true;
}
