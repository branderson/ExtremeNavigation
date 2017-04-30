using System.Collections.Generic;

namespace TiledLoader.Examples._3D.Player.Terminal
{
    public class TerminalCommands
    {
        public static Dictionary<int, string> Commands = new Dictionary<int, string>()
        {
            { 1, "door [-o] [--open] [-p code] [--passcode code] id\n" +
                 "  code: passcode for door, if required"},
            { 2, "door [-c] [--close] id"},
            { 3, "enemy [-s speed] [--speed speed] id\n" +
                 "  speed: 1 (slow), 2 (medium), or 3 (fast)"},
            { 4, "enemy [-r] [--reverse] id" },
        };
    }
}