using System.Linq;
using Data;
using TiledLoader;
using UnityEditor;
using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class LayoutImporter : MonoBehaviour
    {
        [SerializeField] private Texture2D _minimapSpritesheet;

        private void HandleInstanceProperties()
        {
            SetMinimapSprite();
            if (GetComponent<RoadTile>() != null)
            {
                SetDirections();
            }
            DestroyImmediate(GetComponent<TiledLoaderProperties>());
            DestroyImmediate(this, true);
        }

        /// <summary>
        /// Create sprite object to display on minimap and set its sprite
        /// </summary>
        private void SetMinimapSprite()
        {
            // Instantiate minimap sprite object
            GameObject minimapSprite = new GameObject("Minimap Sprite");
            minimapSprite.transform.parent = transform;
            minimapSprite.transform.localPosition = Vector3.zero;
            minimapSprite.transform.localRotation = Quaternion.identity;
            minimapSprite.layer = LayerMask.NameToLayer("Minimap");
            SpriteRenderer sprite = minimapSprite.AddComponent<SpriteRenderer>();

            // Get index of tile in tileset
            int tileIndex;
            string indexString = new string(name.Where(item => char.IsDigit(item)).ToArray());
            int.TryParse(indexString, out tileIndex);

            // Get tileset as array of sprites
            string tilesetPath = AssetDatabase.GetAssetPath(_minimapSpritesheet);
            Sprite[] tilesetSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(tilesetPath).OfType<Sprite>().ToArray();

            if (tileIndex >= tilesetSprites.Length) return;

            // Replace sprite
            sprite.sprite = tilesetSprites[tileIndex];
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