using System.IO;
using Controllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class RenderTextureToPNG : MonoBehaviour
    {
        [MenuItem("Assets/SaveToPNG")]
        public static void SaveToPNG()
        {
            // Initialize minimap camera
            MinimapCameraController camera = FindObjectOfType<MinimapCameraController>();
            camera.SetSize();

            // Load RenderTexture
            RenderTexture rt = Selection.activeObject as RenderTexture;
            string path = Application.dataPath + "/Sprites/MinimapRenders/" + SceneManager.GetActiveScene().name + ".png";
            if (rt == null) return;
            if (!rt.IsCreated())
            {
                Debug.Log("RenderTexture not created");
                return;
            }
            RenderTexture.active = rt;

            // Read RenderTexture into Texture2D
            Texture2D texture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply();
            RenderTexture.active = null;

            // Encode RenderTexture to PNG
            byte[] bytes = texture.EncodeToPNG();
            DestroyImmediate(texture, true);
            Directory.CreateDirectory(Application.dataPath + "/Sprites/MinimapRenders/");
            File.WriteAllBytes(path + ".png", bytes);
        }

        [MenuItem("Assets/SaveToPNG", true)]
        public static bool SaveToPNGValidation()
        {
            if (Selection.activeObject == null) return false;
            return Selection.activeObject.GetType() == typeof(RenderTexture);
        }
    }
}
