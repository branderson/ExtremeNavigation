using Controllers;

namespace Data
{
    public class PathNode
    {
        public RoadTileController Data = null;
        public PathNode Next = null;
        public PathNode Prev = null;

        public PathNode(RoadTileController tile)
        {
            Data = tile;
        }

        /// <summary>
        /// Append a node with the given RoadTileController to the end of the list
        /// </summary>
        /// <param name="tile">
        /// Tile to append
        /// </param>
        /// <returns>
        /// PathNode of new head of list
        /// </returns>
        public PathNode MoveTo(RoadTileController tile)
        {
            Next = new PathNode(tile) {Prev = this};
            return Next;
        }

        /// <summary>
        /// Remove this node and all nodes after it from the list
        /// </summary>
        /// <returns>
        /// PathNode of new head of list
        /// </returns>
        public PathNode Pop()
        {
            if (Next != null) Next.Pop();
            Data = null;
            Next = null;
            Prev.Next = null;
            return Prev;
        }
    }
}