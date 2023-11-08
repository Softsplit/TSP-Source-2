
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Sandbox.UI.Construct;

namespace TSPS2.Menu;

public class ObjectEditor : Panel
{
    public object Target
    {
        get => target;
        set
        {
            if (value.Equals(target))
                return;

            BuildForTarget(value);
            target = value;
        }
    }
    private object target;

    public ObjectEditor()
    {
        Style.FlexDirection = FlexDirection.Column;
    }

    private void BuildForTarget(object target)
    {
        DeleteChildren(true);

        var properties = TypeLibrary.GetPropertyDescriptions(target);

        //Sort all properties based on Display Info Group value
        Dictionary<string, List<PropertyDescription>> groupedProperties = new Dictionary<string, List<PropertyDescription>>();
        foreach (var property in properties)
        {
            var displayInfo = property.GetDisplayInfo();

            if (!property.IsStatic)
            {
                string group = displayInfo.Group;

                if (!groupedProperties.TryGetValue(group, out var groupPropertyList))
                {
                    groupPropertyList = new List<PropertyDescription>();
                    groupedProperties.Add(group, groupPropertyList);
                }

                groupPropertyList.Add(property);
            }
        }

        //Sort the grouped properties
        var groups = new List<KeyValuePair<string, List<PropertyDescription>>>(groupedProperties);
        groups.Sort((a, b) =>
        {
            int orderA = TSPClientSettings.GetGroupOrder(a.Key);
            int orderB = TSPClientSettings.GetGroupOrder(b.Key);

            return orderA.CompareTo(orderB);
        });

        //Loop over all groups
        foreach (var propertyGroup in groups)
        {
            Panel group = Add.Panel("group");
            //Make heading
            Label header = group.Add.Label(classname: "header");

            header.SetContent(propertyGroup.Key);

            //Add settings row for properties
            foreach (var groupProperty in propertyGroup.Value)
            {
                group.AddChild(new SettingRow(target, groupProperty) { Classes = "item" });
            }
        }
    }
}