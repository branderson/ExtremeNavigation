using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    [ExecuteInEditMode]
    public class DoorInitializer : MonoBehaviour
    {
        public void HandleInstanceProperties()
        {
            Door door = GetComponent<Door>();
            LoadedText comp = GetComponent<LoadedText>();
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();
            int id;
            bool open;
            string passcode;
            properties.TryGetInt("ID", out id);
            properties.TryGetBool("Open", out open);
            properties.TryGetString("Passcode", out passcode);
            passcode = passcode.Replace(" ", "");
            door.ID = id;
            door.Open = open;
            door.Passcode = passcode;
            if (passcode == "")
            {
                comp.Text = "Door " + id;
            }
            else
            {
                comp.Text = "[Locked] Door " + id;
            }
            DestroyImmediate(this);
        }
    }
}