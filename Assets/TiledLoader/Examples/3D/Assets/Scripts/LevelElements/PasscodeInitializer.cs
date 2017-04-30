using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    [ExecuteInEditMode]
    public class PasscodeInitializer : MonoBehaviour
    {
        private void HandleInstanceProperties()
        {
            Passcode pass = GetComponent<Passcode>();
            LoadedText comp = GetComponent<LoadedText>();
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();
            string text;
            int id;
            properties.TryGetString("Code", out text);
            properties.TryGetInt("ID", out id);
            pass.ID = id;
            pass.Code = text;
            comp.Text = "Door: " + id +  "\n" + "Code: " + text;
            DestroyImmediate(this);
        }
    }
}