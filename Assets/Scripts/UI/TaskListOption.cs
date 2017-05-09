using System.Linq;
using TiledLoader;
using UI.Components;
using UnityEngine.UI;

namespace UI
{
    public sealed class TaskListOption : ListOption
    {
        private readonly string red = "#EA3F3F";
        private readonly string yellow = "#FFDE2A";
        private readonly string green = "#A8D139";

        public override void Configure(TiledLoaderPropertyData properties)
        {
            Properties = properties;
            // Load properties
            string name;
            int value;
            string description;
            int count;
            Properties.TryGetString("Name", out name);
            Properties.TryGetInt("Value", out value);
            Properties.TryGetString("Description", out description);
            Properties.TryGetInt("Count", out count);

            gameObject.name = name;

            // Load components
            Text nameText = GetComponentsInChildren<Text>().FirstOrDefault(item => item.name == "Name");
            Text valueText = GetComponentsInChildren<Text>().FirstOrDefault(item => item.name == "Value");
            Text countText = GetComponentsInChildren<Text>().FirstOrDefault(item => item.name == "Count");
            Text descriptionText = GetComponentsInChildren<Text>().FirstOrDefault(item => item.name == "Description");

            nameText.text = name;
            valueText.text = "$" + value;
            string val = "";
            // Add marker indicators
            if (count > 1) val += " <color=" + green + ">@</color>";
            if (count > 2) val += " <color=" + yellow + ">@</color>";
            val += " <color=" + red + ">@</color>";
            countText.text = val;
            descriptionText.text = description;
        }
    }
}