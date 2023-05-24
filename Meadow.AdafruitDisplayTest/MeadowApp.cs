using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Meadow.AdafruitDisplayTest
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        MicroGraphics graphics;
        IGraphicsDisplay display;
        IProjectLabHardware? plab;
        
        // *** Setup up notes for this test app ***
        // Change this to the display you're using for test
        // AdaFruit_170x320 is the 1.9" 170x320 IPS TFT display
        // Adafruit_240x320 is the 2.0" 240x320 IPS TFT display
        // ProjectLab_240x240 is the 1.54" 240x240 IPS TFT display on the PLAB board
        readonly DisplayParams displayParams = AdafruitDisplay.ProjectLab_240x240;

        // Set this to true to run all tests, or false to run just the line number test
        readonly bool runAllTests = true;

        // Set the delay between tests in order to view results
        readonly TimeSpan interTestDelay = TimeSpan.FromSeconds(5);

        public override Task Initialize()
        {
            Resolver.Log.Loglevel = Logging.LogLevel.Trace;
            Resolver.Log.Info("Adafruit test initializing...");

            if (displayParams.IsProjectLabHardware)
            {
                plab = ProjectLab.Create();
                display = plab.Display;
                graphics = CreateGraphicsDisplay(display, displayParams.DisplayRotation);
            }
            else
            {
                var spiBus = CreateDisplaySpiBus(displayParams);
                display = CreateAdafruitDisplay(spiBus, displayParams);
                graphics = CreateGraphicsDisplay(display, displayParams.DisplayRotation);
            }

            Resolver.Log.Info("Adafruit test initialize complete.");

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Adafruit test running...");

            while (true)
            {
                LineNumberTest();
                Thread.Sleep(interTestDelay);

                if (runAllTests)
                {
                    FontScaleTest();
                    Thread.Sleep(interTestDelay);

                    FontAlignmentTest();
                    Thread.Sleep(interTestDelay);

                    ColorFontTest();
                    Thread.Sleep(interTestDelay);

                    BufferRotationTest();
                    Thread.Sleep(interTestDelay);

                    PathTest();
                    Thread.Sleep(interTestDelay);

                    LineTest();
                    Thread.Sleep(interTestDelay);

                    PolarLineTest();
                    Thread.Sleep(interTestDelay);
                }
            }
        }

        #region Test methods

        void LineNumberTest()
        {
            graphics.Clear(true);

            var font = new Font8x12();
            int x = 0, y = 0, i = 0;

            Resolver.Log.Trace($"...line number test starting...");
            Resolver.Log.Trace($"......font height is {font.Height}px, display height is {display.Height}, total lines = {Math.Abs(display.Height/font.Height)}...");

            do
            {
                graphics.DrawText(x, y, i.ToString(), Color.White, font: font);
                Resolver.Log.Trace($"y = {y}, i = {i}");
                y += font.Height;
                i += 1;
            }
            while (y < graphics.Height);

            graphics.Show();

            Resolver.Log.Trace($"...line number test complete.");
        }

        void FontScaleTest()
        {
            Resolver.Log.Trace("...font scale test starting...");

            graphics.CurrentFont = new Font12x20();

            graphics.Clear();

            graphics.DrawText(0, 0, "2x Scale", Color.Blue, ScaleFactor.X2);
            graphics.DrawText(0, 48, "12x20 Font", Color.Green, ScaleFactor.X2);
            graphics.DrawText(0, 96, "0123456789", Color.Yellow, ScaleFactor.X2);
            graphics.DrawText(0, 144, "!@#$%^&*()", Color.Orange, ScaleFactor.X2);
            graphics.DrawText(0, 192, "3x!", Color.OrangeRed, ScaleFactor.X3);
            graphics.DrawText(0, 240, "Meadow!", Color.Red, ScaleFactor.X2);
            graphics.DrawText(0, 288, "B4.2", Color.Violet, ScaleFactor.X2);

            graphics.Show();

            Resolver.Log.Trace("...font scale test complete.");
        }

        void FontAlignmentTest()
        {
            Resolver.Log.Trace("...font alignment test starting...");

            graphics.Clear();

            graphics.DrawText(120, 0, "Left aligned", Color.Blue);
            graphics.DrawText(120, 16, "Center aligned", Color.Green, ScaleFactor.X1, HorizontalAlignment.Center);
            graphics.DrawText(120, 32, "Right aligned", Color.Red, ScaleFactor.X1, HorizontalAlignment.Right);

            graphics.DrawText(120, 64, "Left aligned", Color.Blue, ScaleFactor.X2);
            graphics.DrawText(120, 96, "Center aligned", Color.Green, ScaleFactor.X2, HorizontalAlignment.Center);
            graphics.DrawText(120, 128, "Right aligned", Color.Red, ScaleFactor.X2, HorizontalAlignment.Right);

            graphics.Show();

            Resolver.Log.Trace("...font alignment test complete.");
        }

        void ColorFontTest()
        {
            Resolver.Log.Trace("...color font test starting...");

            graphics.CurrentFont = new Font8x12();

            graphics.Clear();

            graphics.DrawTriangle(120, 20, 200, 100, 120, 100, Meadow.Foundation.Color.Red, false);
            graphics.DrawRectangle(140, 30, 40, 90, Meadow.Foundation.Color.Yellow, false);
            graphics.DrawCircle(160, 80, 40, Meadow.Foundation.Color.Cyan, false);

            int indent = 5;
            int spacing = 14;
            int y = indent;

            graphics.DrawText(indent, y, "Meadow F7 SPI ST7789!!");
            graphics.DrawText(indent, y += spacing, "Red", Meadow.Foundation.Color.Red);
            graphics.DrawText(indent, y += spacing, "Purple", Meadow.Foundation.Color.Purple);
            graphics.DrawText(indent, y += spacing, "BlueViolet", Meadow.Foundation.Color.BlueViolet);
            graphics.DrawText(indent, y += spacing, "Blue", Meadow.Foundation.Color.Blue);
            graphics.DrawText(indent, y += spacing, "Cyan", Meadow.Foundation.Color.Cyan);
            graphics.DrawText(indent, y += spacing, "LawnGreen", Meadow.Foundation.Color.LawnGreen);
            graphics.DrawText(indent, y += spacing, "GreenYellow", Meadow.Foundation.Color.GreenYellow);
            graphics.DrawText(indent, y += spacing, "Yellow", Meadow.Foundation.Color.Yellow);
            graphics.DrawText(indent, y += spacing, "Orange", Meadow.Foundation.Color.Orange);
            graphics.DrawText(indent, y += spacing, "Brown", Meadow.Foundation.Color.Brown);

            graphics.Show();

            Resolver.Log.Info("...color font test complete.");
        }

        void BufferRotationTest()
        {
            Resolver.Log.Trace($"...buffer rotation test starting...");

            var buffer = new BufferRgb888(50, 50);
            var oldRotation = graphics.Rotation;

            graphics.Clear();
            graphics.Rotation = RotationType.Default;
            buffer.Fill(Color.Red);
            graphics.DrawBuffer(10, 10, buffer);

            graphics.Rotation = RotationType._90Degrees;
            buffer.Fill(Color.Green);
            graphics.DrawBuffer(10, 10, buffer);

            graphics.Rotation = RotationType._180Degrees;
            buffer.Fill(Color.Blue);
            graphics.DrawBuffer(10, 10, buffer);

            graphics.Rotation = RotationType._270Degrees;
            buffer.Fill(Color.Yellow);
            graphics.DrawBuffer(10, 10, buffer);

            graphics.Show();

            graphics.Rotation = oldRotation;

            Resolver.Log.Trace("...buffer rotation test complete.");
        }

        void PathTest()
        {
            Resolver.Log.Trace($"...path test starting...");

            var pathSin = new GraphicsPath();
            var pathCos = new GraphicsPath();

            for (int i = 0; i < 48; i++)
            {
                if (i == 0)
                {
                    pathSin.MoveTo(0, 120 + (int)(Math.Sin(i * 10 * Math.PI / 180) * 100));
                    pathCos.MoveTo(0, 120 + (int)(Math.Cos(i * 10 * Math.PI / 180) * 100));
                    continue;
                }

                pathSin.LineTo(i * 5, 120 + (int)(Math.Sin(i * 10 * Math.PI / 180) * 100));
                pathCos.LineTo(i * 5, 120 + (int)(Math.Cos(i * 10 * Math.PI / 180) * 100));
            }

            graphics.Clear();

            graphics.Stroke = 3;
            graphics.DrawLine(0, 120, 240, 120, Color.White);
            graphics.DrawPath(pathSin, Color.Cyan);
            graphics.DrawPath(pathCos, Color.LawnGreen);

            graphics.Show();

            Resolver.Log.Trace("...path test complete.");
        }

        void LineTest()
        {
            Resolver.Log.Trace($"...draw lines test starting...");

            Resolver.Log.Info("Horizonal lines");

            graphics.Clear();

            for (int i = 1; i < 10; i++)
            {
                graphics.Stroke = i;
                graphics.DrawHorizontalLine(5, 20 * i, (graphics.Width - 10), Color.Red);
            }
            graphics.Show();
            Thread.Sleep(1500);

            graphics.Clear();
            Resolver.Log.Info("Horizonal lines (negative)");
            for (int i = 1; i < 10; i++)
            {
                graphics.Stroke = i;
                graphics.DrawHorizontalLine(graphics.Width - 5, 20 * i, 10 - graphics.Width, Color.Green);
            }
            graphics.Show();
            Thread.Sleep(1500);
            graphics.Clear();

            Resolver.Log.Info("Vertical lines");

            graphics.Clear();

            for (int i = 1; i < 10; i++)
            {
                graphics.Stroke = i;
                graphics.DrawVerticalLine(20 * i, 5, graphics.Height - 10, Color.Orange);
            }
            graphics.Show();
            Thread.Sleep(1500);
            graphics.Clear();

            Resolver.Log.Info("Vertical lines (negative)");
            for (int i = 1; i < 10; i++)
            {
                graphics.Stroke = i;
                graphics.DrawVerticalLine(20 * i, graphics.Height - 5, 10 - graphics.Width, Color.Blue);
            }
            graphics.Show();
            Thread.Sleep(1500);

            Resolver.Log.Trace("...draw lines test complete.");
        }

        void PolarLineTest()
        {
            Resolver.Log.Trace("...polar lines test starting...");

            graphics.Clear();
            graphics.Stroke = 3;

            for (int i = 0; i < 270; i += 12)
            {
                graphics.DrawLine(120, 120, 80, (float)(i * Math.PI / 180), Color.White);
            }

            graphics.Show();

            Resolver.Log.Trace("...polar lines test complete.");
        }

        #endregion Test methods

        #region Initialization methods

        ISpiBus CreateDisplaySpiBus(DisplayParams displayParams)
        {
            Resolver.Log.Trace("...creating SPI bus...");
            bool useAdvancedSpiParams = false;

            if (useAdvancedSpiParams == false)
            {
                Resolver.Log.Trace("...default SPI bus created.");
                return Device.CreateSpiBus();
            }
            else
            {
                var spiFreq = new Frequency(48000, Frequency.UnitType.Kilohertz);
                var spiCfg = new SpiClockConfiguration(spiFreq, SpiClockConfiguration.Mode.Mode0);

                var spiBus = Device.CreateSpiBus(
                    Device.Pins.SCK,
                    Device.Pins.COPI,
                    Device.Pins.CIPO,
                    spiCfg
                    );

                Resolver.Log.Trace("...Advanced parameter SPI bus created.");
                return spiBus;
            }
        }

        IGraphicsDisplay CreateAdafruitDisplay(ISpiBus spiBus, DisplayParams dspParams)
        {
            Resolver.Log.Trace($"...creating St7789: width = {dspParams.DisplayWidth}px, height = {dspParams.DisplayHeight}px...");

            St7789 disp = new St7789(
                    spiBus: spiBus,
                    chipSelectPin: Device.Pins.A03,
                    dcPin: Device.Pins.A04,
                    resetPin: Device.Pins.A05,
                    width: dspParams.DisplayWidth, height: dspParams.DisplayHeight);

            disp.Clear(Color.AliceBlue);
            disp.Show();

            Resolver.Log.Trace("...St7789 created.");
            return disp;
        }

        MicroGraphics CreateGraphicsDisplay(IGraphicsDisplay display, RotationType rotation)
        {
            Resolver.Log.Trace($"...creating graphics: rotation is {rotation} degrees...");

            var gpx = new MicroGraphics(display)
            {
                Rotation = rotation,
                IgnoreOutOfBoundsPixels = true
            };

            Resolver.Log.Trace("...graphics created.");
            return gpx;
        }

        #endregion Initialization methods
    }
}