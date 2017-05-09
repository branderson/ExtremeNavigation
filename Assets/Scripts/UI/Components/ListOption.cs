using TiledLoader;
using UnityEngine;

namespace UI.Components
{
    public class ListOption : MonoBehaviour
    {
        // TODO: Maybe don't use TiledLoaderPropertyData here so this can be used without TiledLoader
        // Using TiledLoaderPropertyData because they're nice type-agnostic dicts
        public TiledLoaderPropertyData Properties;

        public virtual void Configure(TiledLoaderPropertyData properties)
        {
            Properties = properties;
        }
    }
}