using Data;
using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class LayoutLayerImporter : MonoBehaviour
    {
        private void HandleLayerProperties()
        {
            BuildRoadGraph();
            DestroyImmediate(this, true);
        }

        private void BuildRoadGraph()
        {
            // Get all road tiles
            RoadTile[] roadTiles = GetComponentsInChildren<RoadTile>();

            // Iterate through each tile
            foreach (RoadTile tile in roadTiles)
            {
                // Check for adjacent tiles in each direction which can connect
                if (tile.GetDirection(RoadTile.Direction.North)) CheckAndSetAdjacency(tile, Vector3.up, RoadTile.Direction.North);
                if (tile.GetDirection(RoadTile.Direction.South)) CheckAndSetAdjacency(tile, Vector3.down, RoadTile.Direction.South);
                if (tile.GetDirection(RoadTile.Direction.East)) CheckAndSetAdjacency(tile, Vector3.right, RoadTile.Direction.East);
                if (tile.GetDirection(RoadTile.Direction.West)) CheckAndSetAdjacency(tile, Vector3.left, RoadTile.Direction.West);
            }
        }

        private void CheckAndSetAdjacency(RoadTile tile, Vector3 offset, RoadTile.Direction direction)
        {
            // Calculate position of potential adjacent tile
            Vector2 checkPos = tile.transform.position + offset;

            // Find all collisions at position
            Collider2D[] cols = Physics2D.OverlapPointAll(checkPos);

            // Check for tile at position
            foreach (Collider2D col in cols)
            {
                // Get RoadTile component if it exists
                RoadTile adjacent = col.GetComponent<RoadTile>();
                if (adjacent == null) continue;

                // To allow for one-way roads, do not check if adjacent can connect
                // Set adjacent
                tile.ConnectEdge(direction, adjacent);
                return;
            }
        }
    }
}