using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest.Components;
using Sandbox;

public class HammerEntity : Component
{
    protected void FireOutput(Outputs output)
    {
        FireOutput(output, "", float.NaN);
    }

    protected void FireOutput(Outputs output, string parameterOverride)
    {
        FireOutput(output, parameterOverride, float.NaN);
    }

    protected void FireOutput(Outputs output, float parameterFloat)
    {
        FireOutput(output, "", parameterFloat);
    }

    protected void FireOutput(Outputs output, string parameterString, float parameterFloat)
    {
        if (!sorted)
        {
            SortOutputsByDelay();
            sorted = true;
        }

        for (int i = 0; i < expandedConnections.Count; i++)
        {
            if (expandedConnections[i].output == output && expandedConnections[i].nestInput != null)
            {
                if (parameterString != "")
                {
                    /*
                    if (expandedConnections[i].nestInput.CurrentEventType == NestInput.EventType.String && expandedConnections[i].nestInput._eventString.GetPersistentEventCount() > 0)
                    {
                        object persistentTarget = expandedConnections[i].nestInput._eventString.GetPersistentTarget(0);

                        MethodDescription method = persistentTarget.GetType().GetMethod(expandedConnections[i].nestInput._eventString.GetPersistentMethodName(0), new TypeDescription[] { typeof(string) });

                        expandedConnections[i].nestInput._eventString = new NestInput.StringEvent();

                        Action<string> action = method.CreateDelegate(typeof(Action<string>), persistentTarget) as Action<string>;

                        expandedConnections[i].nestInput._eventString.AddListener(action);
                    }
                    */

                    expandedConnections[i].nestInput._parameterString = parameterString;
                }

                if (!float.IsNaN(parameterFloat))
                {
                    /*
                    if (expandedConnections[i].nestInput.CurrentEventType == NestInput.EventType.Float && expandedConnections[i].nestInput._eventValue.GetPersistentEventCount() > 0)
                    {
                        object persistentTarget2 = expandedConnections[i].nestInput._eventValue.GetPersistentTarget(0);

                        MethodDescription method2 = persistentTarget2.GetType().GetMethod(expandedConnections[i].nestInput._eventValue.GetPersistentMethodName(0), new TypeDescription[] { typeof(float) });

                        expandedConnections[i].nestInput._eventValue = new NestInput.ValueEvent();

                        Action<float> action2 = method2.CreateDelegate(typeof(Action<float>), persistentTarget2) as Action<float>;

                        expandedConnections[i].nestInput._eventValue.AddListener(action2);
                    }
                    */

                    expandedConnections[i].nestInput._parameterFloat = parameterFloat;
                }

                expandedConnections[i].nestInput.Invoke();
            }
        }
    }

    private void SortOutputsByDelay()
    {
        List<HammerConnection> list = new List<HammerConnection>();

        while (expandedConnections.Count > 0)
        {
            float num = float.MaxValue;
            int num2 = -1;

            for (int i = 0; i < expandedConnections.Count; i++)
            {
                if (expandedConnections[i].delay < num)
                {
                    num = expandedConnections[i].delay;
                    num2 = i;
                }
            }

            if (num2 != -1)
            {
                list.Add(expandedConnections[num2]);
                expandedConnections.Remove(expandedConnections[num2]);
            }
        }

        expandedConnections = list;
    }

    protected override void OnValidate()
    {
        int i;
        int j;

        for (i = 0; i < expandedConnections.Count; i = j + 1)
        {
            new List<Component>((IEnumerable<Component>)Components.Get<Component>()).FindIndex((Component x) => x == expandedConnections[i].nestInput);
            j = i;
        }
    }

    private async Task LogOutput(HammerConnection connection, string output, string filter)
    {
        await GameTask.DelaySeconds(connection.nestInput.Delay);
        string text = " ::: ";
        string text2 = "                              ";
        string text3 = text + "              OUTPUT              " + text;
        string text4;

        if (connection.recipientObject == null)
        {
            text4 = "DESTROYED";
        }
        else
        {
            text4 = connection.recipientObject.Name;
        }

        string text5;

        if (connection.nestInput._parameterString != "")
        {
            text5 = connection.nestInput._parameterString;
        }
        else
        {
            text5 = connection.nestInput._parameterFloat.ToString();
        }

        text3 = text3 + GameObject.Name + text2.Substring(GameObject.Name.Length) + text;
        text3 = text3 + output + text2.Substring(output.Length) + text;
        text3 = text3 + text4 + text2.Substring(text4.Length) + text;
        text3 = text3 + connection.input.ToString() + text2.Substring(connection.input.ToString().Length) + text;
        text3 = text3 + text5 + text2.Substring(text5.Length) + text;
        text3 += connection.nestInput.Delay.ToString();

        if (!(filter == ""))
        {
            text3.Contains(filter);
        }
    }

    public virtual void Use()
    {
    }

    public virtual void Input_Enable()
    {
        isEnabled = true;
    }

    public virtual void Input_Disable()
    {
        isEnabled = false;
    }

    public virtual void Input_Kill()
    {
        GameObject.Enabled = false;
    }

    public void Input_FireUser1()
    {
        FireOutput(Outputs.OnUser1);
    }

    public void Input_FireUser2()
    {
        FireOutput(Outputs.OnUser2);
    }

    public void Input_FireUser3()
    {
        FireOutput(Outputs.OnUser3);
    }

    public void Input_FireUser4()
    {
        FireOutput(Outputs.OnUser4);
    }

    [Property]
    public List<HammerConnection> connections { get; set; } = new List<HammerConnection>();

    [Property]
    public List<HammerConnection> expandedConnections { get; set; } = new List<HammerConnection>();

    public GameObject parent;

    [Property]
    public bool isEnabled { get; set; } = true;

    private bool sorted;
}