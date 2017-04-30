using System;
using System.Linq;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    [ExecuteInEditMode]
    public class SwitchInitializer : MonoBehaviour
    {
        public void HandleInstanceProperties()
        {
            Switch sw = GetComponent<Switch>();
            LoadedText text = GetComponent<LoadedText>();
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();
            int id;
            string doors;
            properties.TryGetInt("ID", out id);
            properties.TryGetString("DoorIDs", out doors);
            sw.ID = id;
            sw.DoorIDs = doors.Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries).Select(item => int.Parse(item)).ToList();
            if (sw.DoorIDs.Count > 1)
            {
                text.Text = "Toggle Doors: " + doors;
            }
            else
            {
                text.Text = "Toggle Door: " + doors;
            }
            DestroyImmediate(this);
        }
    }
}