using System;
using System.Collections.Generic;
using System.Linq;
using TiledLoader.Examples._3D.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TiledLoader.Examples._3D.Player.Terminal
{
    public class TerminalController : MonoBehaviour
    {
        public const char AsciiBold = (char)29;
        public const char AsciiItalic = (char)30;
        public const char AsciiPage = (char)28;
        public const char AsciiCancelText = (char)'`';
        public const char AsciiIgnoreString = (char)31;
        public const char AsciiToggleFontSize = '~';
        public const char AsciiPause = (char)26;

//        [SerializeField] private MainMenuController _menuController;
        [SerializeField] private TerminalScreenController _screen;
        [SerializeField] private AudioClip _keyStroke;
        [SerializeField] private AudioClip _return;
        [SerializeField] private AudioClip _space;
        [SerializeField] private AudioClip _backspace;
        [SerializeField] private AudioClip _newPaper;
        [SerializeField] private string _promptString = "$ >";

        private Text _text;
        private AudioSource _audio;
        private FontStyle _fontStyle;
        private string _typeString;         // String which is being typed to the terminal
        private bool _acceptInput;          // Is the typewriter accepting player input
        private bool _inputDisabled;        // Is the typewriter allowed to accept player input
        private bool _ignoreString;         // Should the typewriter attempt to decipher lines
        private int _waitTimer;             // Frames that must elapse before another character can be handled
        private int _allowedBackspaces;     // Number of characters which may be backspaced
        private int _lineChars;             // Number of characters typed this line
        private int _lines;                 // Number of lines typed this page

        private List<string> _scripts;
        private Dictionary<int, string> _passcodes; 

        public void AddScriptInfo(string text)
        {
            if (_scripts == null)
            {
                _scripts = new List<string>();
            }
            _scripts.Add(text);
            Debug.Log("Added command " + text);
        }

        public void AddPasscode(int id, string code)
        {
            if (_passcodes == null)
            {
                _passcodes = new Dictionary<int, string>();
            }
            _passcodes[id] = code;
            Debug.Log("Added passcode " + code);
        }

        public void Start()
        {
            if (_scripts == null)
            {
                _scripts = new List<string>();
            }
            if (_passcodes == null)
            {
                _passcodes = new Dictionary<int, string>();
            }
            _audio = GetComponent<AudioSource>();
            _text = _screen.Text;
            _typeString = null;
            _acceptInput = true;
            _inputDisabled = false;
            _ignoreString = false;
            _allowedBackspaces = 0;
            _fontStyle = _text.fontStyle;

            _text.text = _promptString;

            TypeString("Type \"passcodes\" to list all learned door codes.\n" +
                       "Type \"man\" to list all learned commands and their usages.\n", false);
        }

        public void Update()
        {
            // If autotyping, autotype
            if (_typeString != null)
            {
                if (_waitTimer-- <= 0)
                {
                    // Ready to handle character
                    if (_typeString.Length == 0)
                    {
                        // End of string
                        _typeString = null;
                        _ignoreString = false;
                        if (!_inputDisabled)
                        {
                            _acceptInput = true;
                        }
                    }
                    else
                    {
                        // Type character to typewriter
                        HandleCharacter(_typeString[0]);
                        _typeString = _typeString.Remove(0, 1);
                    }
                }
            }
            // Handle user input
            if (_acceptInput && Input.anyKeyDown)
            {
                if (Input.inputString.Length <= 0) return;
                // This throws out LF from CRLF
                char c = Input.inputString[0];
                // Commands called before being read in input string
                if (c == AsciiCancelText)
                {
                    _typeString = null;
                    _ignoreString = false;
                    if (!_inputDisabled)
                    {
                        _acceptInput = true;
                    }
                }
                else
                {
                    _typeString += c;
                    //                        if (c == '\b') { } // Prevent checking char info for backspace
                    //                        // Catch carriage return from keyboard (The CR in CRLF in Windows
                    //                        else if (c == '\n' || c == 13 || 
                    //                            _screen.GetLineWidth(_lineChars) + _screen.GetTextWidth(_typeString) >= _maxLineWidth)
                    //                        {
                    //    //                        PreventInputWhileAutotyping();
                    //                        }
                }
            }
            else if (Input.anyKeyDown)
            {
                if (Input.inputString.Length <= 0) return;
                // Commands that ignore _acceptInput
                char c = Input.inputString[0];
                if (c == AsciiCancelText)
                {
                    _typeString = null;
                    _ignoreString = false;
                    if (!_inputDisabled)
                    {
                        _acceptInput = true;
                    }
                }
            }
        }

        /// <summary>
        /// Type out the given string on the typewriter
        /// </summary>
        /// <remarks>
        /// Disables input while typing, preserving previous input restrictions after finished
        /// </remarks>
        /// <param name="s">
        /// String to type
        /// </param>
        /// <param name="clearString">
        /// Whether to clear the contents of the type queue or append to the end of it
        /// </param>
        public void TypeString(string s, bool clearString=true)
        {
            // TODO: Check if string will fit on page, new page if not
            // Go to new line if we need to
            if (_allowedBackspaces > 0)
            {
                HandleCharacter('\n');
            }
            if (!_inputDisabled)
            {
                PreventInputWhileAutotyping();
            }
            // Do not try to decipher string
            if (clearString)
            {
                _typeString = s;
            }
            else
            {
                _typeString += s;
            }
            _ignoreString = true;
        }

        public void HandleCharacter(char c)
        {
            // TODO: Implement these or get rid of them
            if (c == AsciiBold)
            {
                // Toggle bold
                switch (_fontStyle)
                {
                    case FontStyle.Normal:
                        break;
                    case FontStyle.Bold:
                        break;
                    case FontStyle.Italic:
                        break;
                    case FontStyle.BoldAndItalic:
                        break;
                }
                _text.fontStyle = _fontStyle;
            }
            else if (c == AsciiItalic)
            {
                // Toggle italic
                switch (_fontStyle)
                {
                    case FontStyle.Normal:
                        break;
                    case FontStyle.Bold:
                        break;
                    case FontStyle.Italic:
                        break;
                    case FontStyle.BoldAndItalic:
                        break;
                }
                _text.fontStyle = _fontStyle;
            }
            else if (c == AsciiPage)
            {
                // New paper
                _lines = 0;
                _text.text = _promptString;
//                _screen.Clear();
            }
            else if (c == AsciiIgnoreString)
            {
                _ignoreString = !_ignoreString;
            }
            else if (c == AsciiToggleFontSize)
            {
                if (_screen.FontMode == FontSizeMode.Normal)
                {
                    _screen.FontMode = FontSizeMode.Small;
                }
                else
                {
                    _screen.FontMode = FontSizeMode.Normal;
                }
            }
            else if (c == AsciiPause)
            {
                // Wait for half a second
                _waitTimer = 30;
            }
            else if (c == '\b')
            {
                if (_text.text.Length > 0 && _allowedBackspaces > 0)
                {
                    _text.text = _text.text.Remove(_text.text.Length - 1);
                    _audio.PlayOneShot(_backspace);
                    _waitTimer = 5;
                    _allowedBackspaces--;
                    _lineChars--;
                }
            }
            // Catch carriage return from keyboard (The CR in CRLF from Windows)
            else if (c == '\n' || c == 13)
            {
                CarriageReturn();
                _text.text += _promptString;
            }
            // Add to current line
            else if (c == ' ')
            {
                if (_screen.GetLineWidth(_lineChars) >= _screen.MaxLineWidth)
                {
                    // Go down a line instead
                    CarriageReturn();
                }
                else
                {
                    _text.text += c;
                    _audio.PlayOneShot(_space);
                    _waitTimer = 2;
                    _allowedBackspaces++;
                    _lineChars++;
                }
            }
            else
            {
                if (_screen.GetLineWidth(_lineChars) >= _screen.MaxLineWidth)
                {
                    if (!_text.text.EndsWith("-") && !_text.text.EndsWith(" "))
                    {
                        // Add a hyphen
                        _text.text += "-";
                        _audio.PlayOneShot(_keyStroke);
                        _waitTimer = 2;
                        _lineChars++;
                    }
                    // Continue on next line
                    CarriageReturn();
                    // Push the character to the front of the queue so it gets read again
                    _typeString = c + _typeString;
                }
                else
                {
                    _text.text += c;
                    _audio.PlayOneShot(_keyStroke);
                    _waitTimer = 2;
                    _allowedBackspaces++;
                    _lineChars++;
                }
            }
        }

        private void CarriageReturn()
        {
            string lineString = _screen.GetLineString(_lineChars);
//            if (_lines > _screen.GetMaxLines())
//            {
//                // New paper
////                _screen.Clear();
//                _lines = 0;
//                _text.text = _promptString;
//            }
//            else
            {
                // New line
                _text.text += '\n';
//                _screen.Clear();
                _waitTimer = 0;
                _lines++;
                _audio.PlayOneShot(_return);
                _lineChars = 0;
                _allowedBackspaces = 0;
            }

            // Don't let the player type anything else until finished
//            PreventInputWhileAutotyping();

            // See if the string meant anything
            if (!_ignoreString)
            {
                DecipherString(lineString);
            }
        }

        private void DecipherString(string str)
        {
            List<string> words = str.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (words.Count < 1) return;

            // Get opcode
            string opcode = words[0];
            words.Remove(opcode);
            Debug.Log("Parsed opcode: " + opcode);

            // Get options
            List<string> options = words.Where(item => item.StartsWith("-")).ToList();
            foreach (string option in options)
            {
                Debug.Log("Parsed option: " + option);
            }
            string target = words.LastOrDefault();
            if (target == null || target.StartsWith("-"))
            {
                target = "";
            }
            else
            {
                words.Remove(target);
            }
            Debug.Log("Parsed target: " + target);

            // Get option values
            Dictionary<string, string> vals = new Dictionary<string, string>();
            foreach (string option in options)
            {
                int optionIndex = words.IndexOf(option);
                if (words.Count > optionIndex + 1)
                {
                    if (!words[optionIndex + 1].StartsWith("-"))
                    {
                        vals[option] = words[optionIndex + 1];
                        Debug.Log("Parsed value for option " + option + ": " + vals[option]);
                    }
                }
                else
                {
                    Debug.Log("Did not parse value for option " + option);
                    vals[option] = "";
                }
            }

            // Parse command
            string output = "";
            switch (opcode)
            {
                case "passcodes":
                    foreach (int id in _passcodes.Keys.OrderByDescending(item => item))
                    {
                        output += "Door " + id + ": " + _passcodes[id] + "\n";
                    }
                    TypeString(output, false);
                    break;
                case "man":
                    foreach (string command in _scripts.OrderBy(item => item))
                    {
                        output += command + "\n";
                    }
                    TypeString(output, false);
                    break;
                case "door":
                    // Look for and retrieve passcode
                    string passcode = "";
                    if (options.Contains("-p"))
                    {
                        passcode = vals["-p"];
                    }
                    else if (options.Contains("--passcode"))
                    {
                        passcode = vals["--passcode"];
                    }
                    if (options.Any(item => item == "-o" || item == "--open"))
                    {
                        Debug.Log("OpenDoor" + target + passcode);
                        EventManager.Instance.TriggerEvent("OpenDoor" + target + passcode);
                    }
                    if (options.Any(item => item == "-c" || item == "--close"))
                    {
                        Debug.Log("CloseDoor" + target + passcode);
                        EventManager.Instance.TriggerEvent("CloseDoor" + target + passcode);
                    }
                    break;
                case "enemy":
                    string speed = "";
                    bool handleSpeed = false;
                    if (options.Contains("-s"))
                    {
                        speed = vals["-s"];
                        handleSpeed = true;
                    }
                    else if (options.Contains("--speed"))
                    {
                        speed = vals["--speed"];
                        handleSpeed = true;
                    }
                    if (handleSpeed)
                    {
                        long speedVal = long.Parse(speed);
                        if (speedVal > 0 && speedVal < 4)
                        {
                            EventManager.Instance.TriggerEvent("EnemySpeed" + target, speedVal);
                        }
                    }
                    if (options.Any(item => item == "-r" || item == "--reverse"))
                    {
                        EventManager.Instance.TriggerEvent("Reverse" + target);
                    }
                    break;
                default:
                    Debug.Log("Unrecognized command");
                    break;
            }
        }

        public void DisableInput()
        {
            _acceptInput = false;
            _inputDisabled = true;
        }

        public void PreventInputWhileAutotyping()
        {
            _acceptInput = false;
        }

        public void EnableInput()
        {
            _inputDisabled = false;
            if (_typeString == null)
            {
                _acceptInput = true;
            }
        }

        public void PreventBackspace()
        {
            _allowedBackspaces = 0;
        }
    }
}