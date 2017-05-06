using Controllers;
using TiledLoader;
using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class MapImporter : MonoBehaviour
    {
        private void HandleMapProperties()
        {
            ConfigureLevel();
            LoadMapBounds();
            DestroyImmediate(GetComponent<TiledLoaderProperties>());
            DestroyImmediate(this, true);
        }

        private void ConfigureLevel()
        {
            // Get reference to TiledLoaderProperties and LevelController
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();
            LevelController level = GetComponent<LevelController>();

            // Initialize level time
            int time;
            properties.TryGetInt("Time", out time);
            level.InitializeTimer(time);
        }

        private void LoadMapBounds()
        {
            // Get all road tiles
            Collider2D[] tiles = GetComponentsInChildren<Collider2D>();
            Rect bounds = new Rect();

            // Iterate through each tile
            foreach (Collider2D tile in tiles)
            {
                // If tile is outside bounds, expand bounds
                if (tile.transform.position.x + .5f > bounds.xMax) bounds.xMax = tile.transform.position.x + .5f;
                if (tile.transform.position.x - .5f < bounds.xMin) bounds.xMin = tile.transform.position.x - .5f;
                if (tile.transform.position.y + .5f > bounds.yMax) bounds.yMax = tile.transform.position.y + .5f;
                if (tile.transform.position.y - .5f < bounds.yMin) bounds.yMin = tile.transform.position.y - .5f;
            }

            LevelController level = GetComponent<LevelController>();
            level.Bounds = bounds;
        }
    }
}