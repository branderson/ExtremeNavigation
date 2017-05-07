using Data;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public RoadTileController CurrentPosition = null;

        private void Awake()
        {
            // TODO: Do this on load
            // Check for all colliding objects
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position);
            foreach (Collider2D col in cols)
            {
                // Check each colliding object to find layout element
                RoadTileController roadTile = col.GetComponent<RoadTileController>();
                if (roadTile != null)
                {
                    CurrentPosition = roadTile;
                }
            }
        }

//        public Move MoveTo(RoadTileController tile)
//        {
//            if (!CanMoveTo(tile)) return Move.Stay;
//            Move move = CurrentPosition.GetMove(tile);
//            if (move == Move.Stay) return Move.Stay;
//            CurrentPosition.UnsetSurroundingAvailable();
//            CurrentPosition = tile;
//            CurrentPosition.MovedTo();
//            transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, transform.position.z);
//            return move;
//        }

//        public bool CanMoveTo(RoadTileController tile)
//        {
//            return CurrentPosition.GetConnected(tile);
//        }
    }
}