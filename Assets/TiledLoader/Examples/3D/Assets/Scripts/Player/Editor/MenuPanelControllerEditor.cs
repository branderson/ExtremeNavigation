using TiledLoader.Examples._3D.Player;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UI.Editor
{
    [CustomEditor(typeof (MenuPanelController))]
    [CanEditMultipleObjects]
    public class MenuPanelControllerEditor : UnityEditor.Editor
    {
        private MenuPanelController _controller;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _controller = (MenuPanelController) target;

            if (GUILayout.Button("Display"))
            {
                _controller.Display();
            }
            if (GUILayout.Button("Hide"))
            {
                _controller.Hide();
            }
        }
    }
}