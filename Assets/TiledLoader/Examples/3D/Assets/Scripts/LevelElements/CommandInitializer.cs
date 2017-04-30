using TiledLoader.Examples._3D.Player.Terminal;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    [ExecuteInEditMode]
    public class CommandInitializer : MonoBehaviour
    {
        private void HandleInstanceProperties()
        {
            LoadedText comp = GetComponent<LoadedText>();
            Command command = GetComponent<Command>();
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();
            int id;
            properties.TryGetInt("ID", out id);
            comp.Text = TerminalCommands.Commands[id];
            command.Text = comp.Text;
            DestroyImmediate(this);
        }
    }
}