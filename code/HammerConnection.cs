using System;
using Sandbox;
using Nest.Components;

[Serializable]
public struct HammerConnection
{
	public string recipient;

	public Outputs output;

	public Inputs input;

	public NestInput nestInput;

	public GameObject recipientObject;

	public string parameter;

	public float delay;

	public bool onceOnly;
}
