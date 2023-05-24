using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meadow.AdafruitDisplayTest
{
    internal class AdafruitDisplay
    {
        /// <summary>
        /// Adafruit 1.9" 170x320 IPS TFT display
        /// </summary>
        public static DisplayParams Adafruit_170x320 = new DisplayParams
        {
            DisplayWidth = 170,
            DisplayHeight = 320,
            DisplayRotation = RotationType.Default
        };

        /// <summary>
        /// Adafruit 2.0" 240x320 IPS TFT display
        /// </summary>
        public static DisplayParams Adafruit_240x320 = new DisplayParams
        {
            DisplayWidth = 240,
            DisplayHeight = 320,
            DisplayRotation = RotationType.Default
        };

        /// <summary>
        /// ProjectLab 1.54" 240x240 IPS TFT display
        /// </summary>
        public static DisplayParams ProjectLab_240x240 = new DisplayParams
        { 
            DisplayHeight = 240, 
            DisplayWidth = 240, 
            DisplayRotation = RotationType.Default,
            IsProjectLabHardware = true
        };
    }

    internal class DisplayParams
    {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; } 
        public RotationType DisplayRotation { get; set; }
        public bool IsProjectLabHardware { get; set; } = false;
    }
}
