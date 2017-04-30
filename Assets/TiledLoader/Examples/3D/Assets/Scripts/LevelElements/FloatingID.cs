using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    public class FloatingID : CustomMonoBehaviour
    {
        [SerializeField] private string _name = "";
        [SerializeField] private bool useID = true;

        private IAddressable _addressable;
        private TextMesh _textMesh;

        private void Awake()
        {
            _textMesh = GetComponent<TextMesh>();
            _addressable = GetComponentInParent<IAddressable>();
            if (useID)
            {
                _textMesh.text = _name + " " + _addressable.ID;
            }
            else
            {
                LoadedText loaded = GetComponentInParent<LoadedText>();
                if (loaded)
                {
                    _textMesh.text = loaded.Text;
                }
            }
        }

        public void SetText(string text)
        {
            _textMesh.text = text;
        }

        private void Update()
        {
            
        }
    }
}