using System.Collections.Generic;
using UnityEngine;

namespace BaboOnLite
{
    public class ColorMark
    {
        //--------------------//
        // ("simbolo", color) //
        //--------------------//
        public List<(string, Color)> colors = new List<(string, Color)>() {
            ("!", Color.red),
            ("?", Color.blue),
            ("*", Color.yellow),
            ("-", Color.black),
            ("$", Color.cyan),
        };
    }
}
