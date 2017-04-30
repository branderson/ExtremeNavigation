using UnityEngine;

namespace TiledLoader.Examples._3D.Enemies
{
    [ExecuteInEditMode]
    public class EnemyInitializer : MonoBehaviour
    {
        private void HandleInstanceProperties()
        {
            EnemyAI ai = GetComponent<EnemyAI>();
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();
            int id;
            int speed;
            properties.TryGetInt("ID", out id);
            properties.TryGetInt("Speed", out speed);
            ai.ID = id;
            ai.SetSpeed(speed);
            properties.TryGetInt("Destination", out ai.Destination);
            DestroyImmediate(this);
        }

    }
}