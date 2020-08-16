using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI;
using System.Diagnostics;
using Windows.UI.Core;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharp.Views.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uwpDrawingTests
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CanvasControl canvasWin2d;
        SKXamlCanvas canvasSkia;

        public MainPage()
        {
            this.InitializeComponent();

            this.canvasWin2d = new CanvasControl();
            this.canvasWin2d.Draw += CanvasWin2d_Draw;
            this.gridForTest1.Children.Add(canvasWin2d);

            this.canvasSkia = new SKXamlCanvas();
            this.canvasSkia.PaintSurface += CanvasSkia_PaintSurface;
            this.gridForTest2.Children.Add(this.canvasSkia);
        }

        private void CanvasSkia_PaintSurface(object sender, SkiaSharp.Views.UWP.SKPaintSurfaceEventArgs args)
        {
            Stopwatch sw = new Stopwatch();

            sw.Restart();

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Colors.Black.ToSKColor(),
                StrokeWidth = 1,
                IsAntialias = true
            };

            SKPoint center = new SKPoint();

            center.X = info.Width / 2;
            center.Y = info.Height / 2;
            float radioMax;

            if (center.X < center.Y)
                radioMax = center.X - 10;
            else
                radioMax = center.Y - 10;

            for (float radio = 2; radio <= radioMax; radio += 1.2f)
                canvas.DrawCircle(center, radio, paint);

            sw.Stop();

            this.textForStatus2.Text = String.Format("Tardamos {0} ms", sw.ElapsedMilliseconds);
        }

        private void CanvasWin2d_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            Stopwatch sw = new Stopwatch();

            sw.Restart();

            CanvasDrawingSession drawingSession = args.DrawingSession;
            Vector2 center;

            center.X = (float)this.gridForTest1.ActualWidth / 2;
            center.Y = (float)this.gridForTest1.ActualHeight / 2;
            float radioMax;

            if (center.X < center.Y)
                radioMax = center.X - 10;
            else
                radioMax = center.Y - 10;

            drawingSession.Antialiasing = CanvasAntialiasing.Antialiased;

            for (float radio = 2; radio <= radioMax; radio += 1.2f)
                drawingSession.DrawCircle(center, radio, Colors.Black);

            sw.Stop();

            this.textForStatus1.Text = String.Format("Tardamos {0} ms", sw.ElapsedMilliseconds);
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            this.canvasWin2d.Invalidate();
            this.canvasSkia.Invalidate();
        }
    }
}
