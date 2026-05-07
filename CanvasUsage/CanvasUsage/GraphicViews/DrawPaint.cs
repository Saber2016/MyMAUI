using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Microsoft.Maui.Graphics.Point;
using PointF = Microsoft.Maui.Graphics.PointF;
using SizeF = Microsoft.Maui.Graphics.SizeF;

namespace CanvasUsage.GraphicViews
{
    /// <summary>
    /// 采用附加属性来实现GraphicsView的自动刷新，避免在MainPage 中手动绑定事件
    /// 如果有大量类似的GraphicsView需要绑定事件，可以考虑使用附加属性来简化代码，减少重复的事件绑定逻辑，提高代码的可维护性和可读性。
    /// 如果你需要很多类似DrawPaint的绘制类，可以考虑创建一个基类来封装公共的绘制逻辑，可以从GraphicsView类派生一个类，并添加依赖项属性来绑定绘制数据，这样可以进一步简化代码并提高可重用性。
    /// </summary>
    public partial class DrawPaint : ObservableObject, IDrawable
    {
        public string SelectedPaint { get; set; } = string.Empty;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            switch (SelectedPaint)
            {
                case "SolidBrush":
                    DrawSolidBrush(canvas, dirtyRect);
                    break;
                // Add cases for other paint types here
                case "ImagePaint":
                    DrawImagePaint(canvas, dirtyRect);
                    break;
                case "PatternPaint":
                    DrawPatternPaint(canvas, dirtyRect);
                    break;
                case "LinearGradientPaint":
                    DrawLinearGradientPaint(canvas, dirtyRect);
                    break;
                case "HorizentalLinearGradientPaint":
                    DrawHorizentalLinearGradientPaint(canvas, dirtyRect);  
                    break;  
                case "RadialGradientPaint":
                    DrawRadialGradientPaint(canvas, dirtyRect);
                    break;
                default:
                    // Optionally, you can clear the canvas or draw a default background
                    canvas.FillColor = Colors.White;
                    canvas.FillRectangle(dirtyRect);
                    break;
            }
        }

        //public event Action? RequestRedraw;

        //partial void OnSelectedPaintChanged(string value)
        //{
        //    RequestRedraw?.Invoke(); // Trigger a redraw when the selected paint changes
        //}

        private static void DrawSolidBrush(ICanvas canvas, RectF dirtyRect)
        {
            // Example of drawing with a solid brush
            SolidPaint solidPaint = new SolidPaint();
            RectF solidRectangle = new RectF(dirtyRect.X + 20, dirtyRect.Y + 20, dirtyRect.Width - 40, dirtyRect.Height - 80);
            canvas.SetFillPaint(solidPaint, solidRectangle);
            canvas.SetShadow(new SizeF(10, 10), 10, Colors.Gray);
            canvas.FillRoundedRectangle(solidRectangle, 12);
        }

        private static async void DrawImagePaint(ICanvas canvas, RectF dirtyRect)
        {
            // Example of drawing with a texture brush
            using Stream stream = await FileSystem.OpenAppPackageFileAsync("gear.png");
            var image = PlatformImage.FromStream(stream);
            if (image == null)
                return;
            ImagePaint imagePaint = new ImagePaint { Image = image.Downsize(100) };
            //canvas.SetFillPaint(imagePaint, RectF.Zero); 可以用下面代码简化
            canvas.SetFillImage(image.Downsize(100));
            canvas.FillRectangle(dirtyRect);
        }

        private static void DrawPatternPaint(ICanvas canvas, RectF dirtyRect)
        {
            // Example of drawing with a pattern brush
            // Note: Microsoft.Maui.Graphics does not have a built-in pattern brush, so this is just a placeholder
            // You would need to implement your own pattern drawing logic here
            // For example, you could create a small bitmap with the pattern and use it as an ImagePaint
            IPattern pattern;
            using (PictureCanvas picture = new PictureCanvas(0, 0, 10, 10))
            {
                picture.StrokeColor = Colors.Silver;
                picture.DrawLine(0, 0, 10, 10);
                picture.DrawLine(10, 0, 0, 10);
                pattern = new PicturePattern(picture.Picture, 10, 10);
            }
            PatternPaint patternPaint = new PatternPaint { Pattern = pattern };
            canvas.SetFillPaint(patternPaint, dirtyRect);
            canvas.FillRectangle(dirtyRect);
        }

        private static void DrawLinearGradientPaint(ICanvas canvas, RectF dirtyRect)
        {
            // Example of drawing with a linear gradient brush
            LinearGradientPaint linearGradientPaint = new LinearGradientPaint
            {
                StartColor = Colors.Yellow,
                EndColor = Colors.Green,
                StartPoint = new PointF(dirtyRect.Left, dirtyRect.Top),
                EndPoint = new PointF(1,1)
            };
            linearGradientPaint.AddOffset(0.25f, Colors.Orange);
            linearGradientPaint.AddOffset(0.75f, Colors.LightGreen);

            canvas.SetFillPaint(linearGradientPaint, dirtyRect);
            canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
            canvas.FillRoundedRectangle(dirtyRect, 12);
        }

        private static void DrawHorizentalLinearGradientPaint(ICanvas canvas, RectF dirtyRect)
        {
            // Example of drawing with a horizontal linear gradient brush
            LinearGradientPaint linearGradientPaint = new LinearGradientPaint
            {
                StartColor = Colors.Yellow,
                EndColor = Colors.Green,
                StartPoint = new PointF(0,0),
                EndPoint = new PointF(1,0)
            };
            linearGradientPaint.AddOffset(0.25f, Colors.Orange);
            linearGradientPaint.AddOffset(0.75f, Colors.LightGreen);
            canvas.SetFillPaint(linearGradientPaint, dirtyRect);
            canvas.SetShadow(new SizeF(10, 10), 10, Colors.Gray);
            RectF rect = new RectF(dirtyRect.X + 20, dirtyRect.Y + 20, dirtyRect.Width - 40, dirtyRect.Height - 40);
            canvas.FillRoundedRectangle(rect, 12);
        }

        private static void DrawRadialGradientPaint(ICanvas canvas, RectF dirtyRect)
        {
            // Example of drawing with a radial gradient brush
            RadialGradientPaint radialGradientPaint = new RadialGradientPaint
            {
                StartColor = Colors.Red,
                EndColor = Colors.Yellow,
                Center = new Point(0, 0),
                //Radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2
            };
            radialGradientPaint.AddOffset(0.25f, Colors.Orange);
            radialGradientPaint.AddOffset(0.75f, Colors.LightGreen);
            canvas.SetFillPaint(radialGradientPaint, dirtyRect);
            canvas.SetShadow(new SizeF(10,10),10, Colors.Gray);
            RectF rect = new RectF(dirtyRect.X + 20, dirtyRect.Y + 20, dirtyRect.Width - 40, dirtyRect.Height - 40);
            canvas.FillRoundedRectangle(rect, 12);
        }
    }
}
