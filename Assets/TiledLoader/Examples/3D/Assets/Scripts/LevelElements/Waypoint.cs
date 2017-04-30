using TiledLoader.Examples._3D.Enemies;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    public class Waypoint : CustomMonoBehaviour {
        public int ID;
        public int NextID;
        public int PreviousID;

        public int Next() {
            if (NextID == 0)
                return -PreviousID;
            return NextID;
        }

        public int Previous()
        {
            if (PreviousID == 0)
                return -NextID;
            return PreviousID;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.name != "Model") return;
            EnemyAI enemy = col.GetComponentInParent<EnemyAI>();
            if (enemy == null) return;
            enemy.WaypointTouched(this);
        }
    }
}
