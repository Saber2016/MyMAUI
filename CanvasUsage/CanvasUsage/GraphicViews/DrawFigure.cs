using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Font = Microsoft.Maui.Graphics.Font;

namespace CanvasUsage.GraphicViews
{
    public partial class DrawFigure : ObservableObject, IDrawable
    {
        [ObservableProperty]
        public partial string CurrentFigure { get; set; } = "Line"; // Default figure to draw

        public event Action? RequestRedraw;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            switch (CurrentFigure)
            {
                case "Line":
                    DrawFigure.DrawLine(canvas, dirtyRect);
                    break;
                case "DashLine":
                    DrawFigure.DrawDashLine(canvas, dirtyRect);
                    break;
                case "Ellipse":
                    DrawFigure.DrawEllipse(canvas, dirtyRect);
                    break;
                case "Circle":
                    DrawFigure.DrawCircle(canvas, dirtyRect);
                    break;
                case "FillEllipse":
                    FillEllipse(canvas, dirtyRect);
                    break;
                case "FillCircle":
                    FillCircle(canvas, dirtyRect);
                    break;
                case "Rectangle":
                    DrawRectangle(canvas, dirtyRect);
                    break;
                case "FillRectangle":
                    FillRectangle(canvas, dirtyRect);
                    break;
                case "RoundRectangle":
                    DrawRoundRectangle(canvas, dirtyRect);
                    break;
                case "FillRoundRectangle":
                    FillRoundRectangle(canvas, dirtyRect);
                    break;
                case "Arc":
                    DrawArc(canvas, dirtyRect);
                    break;
                case "Arc2":
                    DrawArc2(canvas, dirtyRect);
                    break;
                case "FillArc":
                    FillArc(canvas, dirtyRect);
                    break;
                case "Path":
                    DrawPath(canvas, dirtyRect);
                    break;
                case "Triangle":
                    DrawTriangle(canvas, dirtyRect);
                    break;
                case "FillTriangle":
                    FillTriangle(canvas, dirtyRect);
                    break;
                case "Image":
                    DrawImage(canvas, dirtyRect);
                    break;
                case "String":
                    DrawString(canvas, dirtyRect);
                    break;
                //case "AttributeText":
                //    DrawAttributeText(canvas, dirtyRect);
                //    break;
                case "Example1":
                    DrawExample1(canvas, dirtyRect);
                    break;
                case "Example2":
                    DrawExample2(canvas, dirtyRect);
                    break;
                case "LineCap":
                    DrawLineCap(canvas, dirtyRect);
                    break;
                case "ClipPath":
                    DrawClipPath(canvas, dirtyRect);
                    break;
                case "ClipPath2":
                    DrawClipPath2(canvas, dirtyRect);
                    break;
                default:
                    DrawFigure.DrawLine(canvas, dirtyRect); // Default to drawing a line
                    break;
            }
        }
        partial void OnCurrentFigureChanged(string value)
        {
            RequestRedraw?.Invoke(); // Request the canvas to redraw when the figure changes   
        }

        private static void DrawLine(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawLine(dirtyRect.X + 10, dirtyRect.Y + 10, dirtyRect.Width - 20, dirtyRect.Height - 20); // Draw a line from (10, 10) to (100, 100)
        }
        private static void DrawDashLine(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeSize = 2;
            canvas.StrokeDashPattern = new float[] { 5, 5 }; // Dash pattern: 5 pixels on, 5 pixels off
            canvas.DrawLine(dirtyRect.X + 10, dirtyRect.Y + 10, dirtyRect.Width - 20, dirtyRect.Height - 20); // Draw a dashed line from (10, 20) to (100, 110)
        }
        private static void DrawEllipse(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Green;
            canvas.StrokeSize = 2;
            canvas.DrawEllipse(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2 - 10); // Draw an ellipse at (10, 10) with width 100 and height 50
        }

        private static void DrawCircle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Purple;
            canvas.StrokeSize = 2;
            canvas.DrawEllipse(dirtyRect.X + 10, dirtyRect.Y + 10, dirtyRect.Width - 20, dirtyRect.Height - 20); // Draw a circle at (10, 10) with diameter 50
        }

        private static void FillEllipse(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Yellow;
            canvas.FillEllipse(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2 - 10); // Fill an ellipse at (10, 10) with width 100 and height 50
        }

        private static void FillCircle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Cyan;
            canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, dirtyRect.Width / 2 - 10); // Fill a circle at (10, 10) with diameter 50
        }
        private static void DrawRectangle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Orange;
            canvas.StrokeSize = 2;
            //float size = Math.Min(dirtyRect.Width, dirtyRect.Height) - 20; // Ensure the square fits within the dirty rectangle
            canvas.DrawRectangle(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2); // Draw a square at (10, 10) with the calculated size
        }
        private static void FillRectangle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Magenta;
            //float size = Math.Min(dirtyRect.Width, dirtyRect.Height) - 20; // Ensure the square fits within the dirty rectangle
            canvas.FillRectangle(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2); // Fill a square at (10, 10) with the calculated size
        }

        private static void DrawRoundRectangle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Brown;
            canvas.StrokeSize = 2;
            float cornerRadius = 20; // Set the corner radius for the rounded rectangle
            canvas.DrawRoundedRectangle(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2, cornerRadius); // Draw a rounded rectangle at (10, 10) with width 100 and height 50
        }

        private static void FillRoundRectangle(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.LightBlue;
            float cornerRadius = 20; // Set the corner radius for the rounded rectangle
            canvas.FillRoundedRectangle(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2, cornerRadius); // Fill a rounded rectangle at (10, 10) with width 100 and height 50
        }

        private static void DrawArc(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.DarkGreen;
            canvas.StrokeSize = 2;
            float startAngle = 0; // Starting angle in degrees
            float sweepAngle = 180; // Sweep angle in degrees
            canvas.DrawArc(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 4, dirtyRect.Width - 20, dirtyRect.Height / 2, startAngle, sweepAngle, true, false); // Draw an arc at (10, 10) with width 100 and height 50
        }

        private static void DrawArc2(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.DarkRed;
            canvas.StrokeSize = 2;
            float startAngle = 0; // Starting angle in degrees
            float sweepAngle = 180; // Sweep angle in degrees
            canvas.DrawArc(dirtyRect.X + 10, dirtyRect.Y, dirtyRect.Width - 20, dirtyRect.Height - 20, startAngle, sweepAngle, true, false); // Draw an arc at (10, 10) with width 100 and height 50
        }

        private static void FillArc(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.DarkBlue;
            float startAngle = 0; // Starting angle in degrees
            float sweepAngle = 120; // Sweep angle in degrees
            canvas.FillArc(dirtyRect.X + 10, dirtyRect.Y, dirtyRect.Width - 20, dirtyRect.Height - 20, startAngle, sweepAngle, true); // Fill an arc at (10, 10) with width 100 and height 50
        }

        private static void DrawPath(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.DarkCyan;
            canvas.StrokeSize = 2;
            // Create a path and add some points to it
            var path = new PathF();
            path.MoveTo(dirtyRect.X + 10, dirtyRect.Y + 10);
            path.LineTo(dirtyRect.X + dirtyRect.Width - 10, dirtyRect.Y + dirtyRect.Height - 10);
            path.LineTo(dirtyRect.X + dirtyRect.Width - 10, dirtyRect.Y + 10);
            path.LineTo(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height - 10);
            path.Close(); // Close the path to create a rectangle
            path.AppendEllipse(dirtyRect); // Add an ellipse to the path
            canvas.DrawPath(path); // Draw the path on the canvas
        }

        private static void DrawTriangle(ICanvas canvas, RectF dirtyRect)
        {
            PathF path = new PathF();
            path.MoveTo(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 2); // Top vertex
            path.LineTo(dirtyRect.X + dirtyRect.Width - 10, dirtyRect.Y + dirtyRect.Height - 10); // Bottom right vertex
            path.LineTo(dirtyRect.X + dirtyRect.Width / 3, dirtyRect.Y + 10); // Bottom left vertex
            path.Close(); // Close the path to create a triangle
            canvas.StrokeColor = Colors.DarkMagenta;
            canvas.StrokeSize = 4;
            canvas.DrawPath(path); // Draw the triangle on the canvas
        }

        private static void FillTriangle(ICanvas canvas, RectF dirtyRect)
        {
            PathF path = new PathF();
            path.MoveTo(dirtyRect.X + 10, dirtyRect.Y + dirtyRect.Height / 2); // Top vertex
            path.LineTo(dirtyRect.X + dirtyRect.Width - 10, dirtyRect.Y + dirtyRect.Height - 10); // Bottom right vertex
            path.LineTo(dirtyRect.X + dirtyRect.Width / 3, dirtyRect.Y + 10); // Bottom left vertex
            path.Close(); // Close the path to create a triangle
            canvas.FillColor = Colors.DarkMagenta;
            canvas.FillPath(path); // Fill the triangle on the canvas
        }

        private static async void DrawImage(ICanvas canvas, RectF dirtyRect)
        {
            // Load an image from a file or resource
            using var stream = await FileSystem.OpenAppPackageFileAsync("leaves.jpg");
            var image = PlatformImage.FromStream(stream);
            if (image == null)
                return;
            // Draw the image on the canvas at a specific location and size
            canvas.DrawImage(image, dirtyRect.X + 10, dirtyRect.Y + 10, dirtyRect.Width - 20, dirtyRect.Height - 20);
        }

        private static void DrawString(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontColor = Colors.Blue;
            canvas.FontSize = 18;

            canvas.Font = Font.Default;
            canvas.DrawString("Text is left aligned.", 20, 20, 380, 100, HorizontalAlignment.Left, VerticalAlignment.Top);
            canvas.DrawString("Text is centered.", 20, 60, 280, 100, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawString("Text is right aligned.", 20, 100, 280, 100, HorizontalAlignment.Right, VerticalAlignment.Top);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString("这个文本长度比较长，因此给定宽度不够会自动换行.", 20, 120, 200, 100, HorizontalAlignment.Left, VerticalAlignment.Top);

            canvas.Font = new Font("Arial");
            canvas.FontColor = Colors.Black;
            canvas.SetShadow(new SizeF(6, 6), 4, Colors.Gray);
            canvas.DrawString("This text has a shadow.", 20, 180, 300, 100, HorizontalAlignment.Left, VerticalAlignment.Top);
        }

        /// <summary>
        /// 绘制带属性文本需要将 Microsoft.Maui.Graphics.Text.Markdig NuGet 包添加到项目中。
        /// 目前不能使用,引入包后会导致编译错误，暂时注释掉这个方法。
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        //private static void DrawAttributeText(ICanvas canvas, RectF dirtyRect)
        //{
        //    // This method is a placeholder for drawing text with specific attributes such as font, color, and alignment.
        //    // You can implement this method to demonstrate how to draw text with various attributes on the canvas.
        //    //canvas.Font = new Font("Arial");
        //    canvas.FontSize = 18;
        //    canvas.FontColor = Colors.Blue;

        //    string markdownText = @"This is *italic text*, **bold text**, __underline text__, and ***bold italic text***.";
        //    IAttributedText attributedText = MarkdownAttributedTextReader.Read(markdownText); // Requires the Microsoft.Maui.Graphics.Text.Markdig package
        //    canvas.DrawText(attributedText, 10, 10, 200, 400);
        //}

        /// <summary>
        /// 用填充和笔画绘制一个图形示例，展示如何组合使用不同的绘图方法来创建复杂的图形。
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        private static void DrawExample1(ICanvas canvas, RectF dirtyRect)
        {
            // This method is a placeholder for drawing a custom example on the canvas.
            // You can implement this method to demonstrate any specific drawing technique or combination of shapes and text.
            float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 4;

            PathF path = new PathF();
            path.AppendCircle(dirtyRect.Center.X, dirtyRect.Center.Y, radius);

            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeSize = 10;
            canvas.FillColor = Colors.Red;

            canvas.FillPath(path);
            canvas.DrawPath(path);
        }
        private static void DrawExample2(ICanvas canvas, RectF dirtyRect)
        {
            // This method is a placeholder for drawing another custom example on the canvas.
            // You can implement this method to demonstrate any specific drawing technique or combination of shapes and text.
            canvas.FillColor = Colors.Red;
            canvas.SetShadow(new SizeF(10, 10), 4, Colors.Grey);
            canvas.FillRectangle(10, 10, 90, 100);

            canvas.FillColor = Colors.Green;
            canvas.SetShadow(new SizeF(10, -10), 4, Colors.Grey);
            canvas.FillEllipse(110, 10, 90, 100);

            canvas.FillColor = Colors.Blue;
            canvas.SetShadow(new SizeF(-10, 10), 4, Colors.Grey);
            canvas.FillRoundedRectangle(110, 110, 90, 100, 25);
        }

        private static void DrawLineCap(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 10;
            canvas.StrokeLineCap = LineCap.Round; // Set the line cap to round
            canvas.DrawLine(dirtyRect.X + 10, dirtyRect.Y + 10, dirtyRect.Width - 10, dirtyRect.Y + 10); // Draw a line from (10, 10) to (100, 100)
            canvas.StrokeLineCap = LineCap.Square; // Set the line cap to square
            canvas.DrawLine(dirtyRect.X + 10, dirtyRect.Y + 30, dirtyRect.Width - 10, dirtyRect.Y + 30); // Draw another line with square caps
            canvas.StrokeLineCap = LineCap.Butt; // Set the line cap to flat
            canvas.DrawLine(dirtyRect.X + 10, dirtyRect.Y + 50, dirtyRect.Width - 10, dirtyRect.Y + 50); // Draw another line with flat caps

            PathF path = new PathF();
            path.MoveTo(10, 60);
            path.LineTo(110, 120);
            path.LineTo(10, 180);

            canvas.StrokeSize = 10;
            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.DrawPath(path);

            PathF path2 = new PathF();
            path2.MoveTo(120, 60);
            path2.LineTo(180, 120);
            path2.LineTo(120, 180);

            canvas.StrokeSize = 10;
            canvas.StrokeColor = Colors.Orange;
            canvas.StrokeLineJoin = LineJoin.Bevel;
            canvas.DrawPath(path2);

            PathF path3 = new PathF();
            path3.MoveTo(60, 60);
            path3.LineTo(10, 120);
            path3.LineTo(60, 180);

            canvas.StrokeSize = 10;
            canvas.StrokeColor = Colors.Lavender;
            canvas.StrokeLineJoin = LineJoin.Miter;
            canvas.DrawPath(path3);
        }

        private static async void DrawClipPath(ICanvas canvas, RectF dirtyRect)
        {
            // This method is a placeholder for demonstrating how to use clipping paths in drawing.
            // You can implement this method to show how to restrict drawing to a specific area using clipping paths.
            using var stream = await FileSystem.OpenAppPackageFileAsync("leaves.jpg");
            var image = PlatformImage.FromStream(stream);
            if (image == null)
                return;
            PathF path = new PathF();
            path.AppendCircle(dirtyRect.Center, dirtyRect.Width / 4);
            canvas.ClipPath(path);  // Must be called before DrawImage
            canvas.DrawImage(image, 10, 10, dirtyRect.Width - 20, dirtyRect.Height - 20);
        }

        private static async void DrawClipPath2(ICanvas canvas, RectF dirtyRect)
        {
            // This method is a placeholder for demonstrating how to use clipping paths in drawing.
            // You can implement this method to show how to restrict drawing to a specific area using clipping paths.
            using var stream = await FileSystem.OpenAppPackageFileAsync("leaves.jpg");
            var image = PlatformImage.FromStream(stream);
            if (image == null)
                return;
            canvas.SubtractFromClip(60,60, 80, 80); // Subtract a rectangle from the clipping region
            canvas.DrawImage(image, 10, 10, dirtyRect.Width - 20, dirtyRect.Height - 20);

        }
    }
}
