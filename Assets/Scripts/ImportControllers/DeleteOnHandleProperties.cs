using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class DeleteOnHandleProperties : MonoBehaviour
    {
        private void HandleInstanceProperties()
        {
            DestroyImmediate(this.gameObject, true);
        }

        private void HandleLayerProperties()
        {
            DestroyImmediate(this.gameObject, true);
        }
    }
}