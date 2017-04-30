using Data;
using TiledLoader;
using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class LayoutImporter : MonoBehaviour
    {
        private void HandleInstanceProperties()
        {
            SetDirections();
            DestroyImmediate(this, true);
        }

        /// <summary>
        /// Set directions which this tile can be connected to other tiles along
        /// </summary>
        private void SetDirections()
        {
            // Get reference to TiledLoaderProperties
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();

            // Get direction properties
            bool north, south, east, west;
            properties.TryGetBool("North", out north);
            properties.TryGetBool("South", out south);
            properties.TryGetBool("East", out east);
            properties.TryGetBool("West", out west);

            // Get reference to RoadTile component
            RoadTile roadTile = GetComponent<RoadTile>();
            if (roadTile == null) return;

            // Set directions
            roadTile.SetDirection(RoadTile.Direction.North, north);
            roadTile.SetDirection(RoadTile.Direction.South, south);
            roadTile.SetDirection(RoadTile.Direction.East, east);
            roadTile.SetDirection(RoadTile.Direction.West, west);
        }
    }
}