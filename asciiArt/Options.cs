using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace asciiArt
{
    class Options
    {
        [Option('p', "path", Required = true, HelpText = "The image path")]
        public string ImagePath { get; set; }

        [Option('c', "color", Default = "White", Required = false, 
            HelpText = "The color of ascii text in terminal which should be [Black,DarkBlue,DarkGreen," +
            "DarkCyan,DarkRed,DarkMagenta,DarkYellow,Gray,DarkGray,Blue,Green,Cyan,Red,Magenta,Yellow,White]")]
        public string Color { get; set; }

        [Option('i', "invert", Default = false, Required = false, HelpText = "True to invert the image")]
        public bool Invert { get; set; }

    }
}
