#if UNITY_EDITOR
using System.Linq;
using Data;
using TiledLoader;
using UnityEditor;
using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class TrafficImporter : MonoBehaviour
    {
        private static string _layoutPrefix = "LayoutTileset_";
        [SerializeField] private int _yellowTime = 1;
        [SerializeField] private int _redTime = 2;
        [SerializeField] private Texture2D _yellowRoadTileset;
        [SerializeField] private Texture2D _redRoadTileset;
        [SerializeField] private Texture2D _yellowMinimapTileset;
        [SerializeField] private Texture2D _redMinimapTileset;

        private void HandleInstanceProperties()
        {
            FindAndConfigureUnderlyingTile();
            DestroyImmediate(this.gameObject, true);
        }

        /// <summary>
        /// Find layout tile under this tile and configure its time and sprite
        /// </summary>
        private void FindAndConfigureUnderlyingTile()
        {
            // Get reference to TiledLoaderProperties
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();

            // Find level of traffic
            int trafficLevel;
            properties.TryGetInt("TrafficLevel", out trafficLevel);
            // Ensure trafficLevel is valid
            if (!(trafficLevel == 1 || trafficLevel == 2)) return;

            // Get reference to underlying layout tile
            GameObject layoutTile = FindUnderlyingWithPrefix(_layoutPrefix);
            // Ignore traffic not on top of layout tile
            if (layoutTile == null) return;

            // Find index of tile sprite in spritesheet
            int tileIndex;
            string indexString = new string(layoutTile.name.Where(item => char.IsDigit(item)).ToArray());
            int.TryParse(indexString, out tileIndex);

            // Replace layout tile beneath traffic with appropriate traffic tile and update time of tile
            RoadTile roadTile = layoutTile.GetComponent<RoadTile>();
            if (trafficLevel == 1)
            {
                ReplaceTile(layoutTile, tileIndex, _yellowRoadTileset, _yellowMinimapTileset);
                roadTile.AddTime(_yellowTime);
            }
            else if (trafficLevel == 2)
            {
                ReplaceTile(layoutTile, tileIndex, _redRoadTileset, _redMinimapTileset);
                roadTile.AddTime(_redTime);
            }
        }

        /// <summary>
        /// Find the first GameObject under this GameObject whose name begins with prefix
        /// </summary>
        /// <param name="prefix">
        /// Start of name to look for in underlying objects
        /// </param>
        /// <returns>
        /// First colliding GameObject found whose name begins with prefix
        /// </returns>
        private GameObject FindUnderlyingWithPrefix(string prefix)
        {
            // Check for all colliding objects
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position);
            foreach (Collider2D col in cols)
            {
                // Check each colliding object to find layout element
                if (col.name.StartsWith(prefix))
                {
                    return col.gameObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Replace sprite of given tile with the tile at tileIndex in replacementTileset
        /// </summary>
        /// <param name="layoutTile">
        /// Tile to replace sprite of
        /// </param>
        /// <param name="tileIndex">
        /// Index in tileset of sprite to replace sprite with
        /// </param>
        /// <param name="replacementTileset">
        /// Tileset of sprite to replace sprite with
        /// </param>
        /// <param name="minimapTileset">
        /// Tileset of minimap sprite to replace minimap sprite with
        /// </param>
        private void ReplaceTile(GameObject layoutTile, int tileIndex, Texture2D replacementTileset, Texture2D minimapTileset)
        {
            // Get this object's SpriteRenderer
            SpriteRenderer layoutSprite = layoutTile.GetComponent<SpriteRenderer>();
            // Get child SpriteRenderer
            SpriteRenderer minimapSprite = layoutTile.GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(item => item != layoutSprite);

            // Get tileset as array of sprites
            string tilesetPath = AssetDatabase.GetAssetPath(replacementTileset);
            Sprite[] tilesetSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(tilesetPath).OfType<Sprite>().ToArray();

            if (tileIndex >= tilesetSprites.Length) return;

            // Replace sprite
            layoutSprite.sprite = tilesetSprites[tileIndex];

            if (minimapSprite != null)
            {
                string minimapTilesetPath = AssetDatabase.GetAssetPath(minimapTileset);
                Sprite[] minimapTilesetSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(minimapTilesetPath).OfType<Sprite>().ToArray();
                // Replace minimapSprite
                minimapSprite.sprite = minimapTilesetSprites[tileIndex];
            }
        }
    }
}
#endif