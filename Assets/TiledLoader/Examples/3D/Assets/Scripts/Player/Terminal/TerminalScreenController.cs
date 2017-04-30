using UnityEngine;
using UnityEngine.UI;

namespace TiledLoader.Examples._3D.Player.Terminal
{
    public enum FontSizeMode
    {
        Small,
        Normal,
    }

    public struct FontSettingData
    {
        public float FeedDistance;
        public int MaxLines;
        public int FontSize;

        public static FontSettingData Small = new FontSettingData()
        {
            MaxLines = 25,
            FontSize = 8,
        };

        public static FontSettingData Normal = new FontSettingData()
        {
            MaxLines = 25,
            FontSize = 25,
        };
    }

    public class TerminalScreenController : MonoBehaviour
    {
        [SerializeField] private TerminalController _terminal;
        [SerializeField] public Text Text;
        [SerializeField] public int MaxLineWidth = 160;
        private FontSizeMode _fontMode;
        private FontSettingData _fontSizeData;

        public FontSizeMode FontMode
        {
            get { return _fontMode; }
            set
            {
                bool updateSize = false;
                if (value == _fontMode) return;
                _fontMode = value;
                switch (value)
                {
                    case FontSizeMode.Normal:
                        _fontSizeData = FontSettingData.Normal;
                        break;
                    case FontSizeMode.Small:
                        _fontSizeData = FontSettingData.Small;
                        break;
                }
                if (updateSize)
                {
                    Text.fontSize = _fontSizeData.FontSize;
                }
            }
        }

        public void Start()
        {
            FontMode = FontSizeMode.Normal;
        }

        public void Update()
        {
        }

        /// <summary>
        /// Gets the last given number of characters in the text body
        /// </summary>
        /// <param name="chars">
        /// Number of characters from the end to start at
        /// </param>
        /// <returns>
        /// String of the last given number of characters in the text body
        /// </returns>
        public string GetLineString(int chars)
        {
            return Text.text.Substring(Text.text.Length - chars);
        }

        /// <summary>
        /// Gets the width of the last given number of characters in the text body
        /// </summary>
        /// <param name="chars">
        /// Number of characters from the end to start at to find the width
        /// </param>
        /// <returns>
        /// Width of the last given number of characters rendered in the text body
        /// </returns>
        public int GetLineWidth(int chars)
        {
            int width = 0;
            Font font = Text.font;
 
            foreach(char c in GetLineString(chars))
            {
                CharacterInfo charInfo;

                // Make sure characters in texture memory
                font.RequestCharactersInTexture(c.ToString(), Text.fontSize, Text.fontStyle);

                if (!font.GetCharacterInfo(c, out charInfo, Text.fontSize))
                {
                    Debug.LogError("[TerminalScreenController] No character info found in font for character: " + 
                        c + " (ASCII " + (int)c + ")");
                }
     
                width += charInfo.advance;
            }
 
            return width; 
        }

        /// <summary>
        /// Gets the width of the given string if it were rendered by the paper
        /// </summary>
        /// <param name="str">
        /// String to get potential width of
        /// </param>
        /// <returns>
        /// Width of string if it were rendered
        /// </returns>
        public int GetTextWidth(string str)
        {
            int width = 0;
            Font font = Text.font;

            // Make sure characters in texture memory
            font.RequestCharactersInTexture(str, Text.fontSize, Text.fontStyle);
 
            foreach(char c in str)
            {
                CharacterInfo charInfo;
                if (!font.GetCharacterInfo(c, out charInfo, Text.fontSize))
                {
                    Debug.LogError("[TerminalScreenController] No character info found in font for character: " + 
                        c + " (ASCII " + (int)c + ")");
                }
     
                width += charInfo.advance;
            }
 
            return width; 
        }

        public int GetMaxLines()
        {
            return _fontSizeData.MaxLines;
        }

        public void Clear()
        {
            Text.text = "$ >";
        }
    }
}