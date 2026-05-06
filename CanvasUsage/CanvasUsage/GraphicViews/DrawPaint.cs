using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasUsage.GraphicViews
{
    public partial class DrawPaint : ObservableObject, IDrawable
    {
        [ObservableProperty]
        public partial string SelectedPaint { get; set; } = string.Empty;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            switch(SelectedPaint)
            {
                case "SolidBrush":
                    DrawSolidBrush(canvas, dirtyRect);
                    break;
                // Add cases for other paint types here
                default:
                    // Optionally, you can clear the canvas or draw a default background
                    canvas.FillColor = Colors.White;
                    canvas.FillRectangle(dirtyRect);
                    break;
            }
        }

        public event Action? RequestRedraw;

        partial void OnSelectedPaintChanged(string value)
        {
            RequestRedraw?.Invoke(); // Trigger a redraw when the selected paint changes
        }

        private static void DrawSolidBrush(ICanvas canvas,RectF dirtyRect)
        {
            // Example of drawing with a solid brush
            SolidPaint solidPaint = new SolidPaint();
            RectF solidRectangle = new RectF(dirtyRect.X + 20, dirtyRect.Y + 20, dirtyRect.Width - 40, dirtyRect.Height - 80);
            canvas.SetFillPaint(solidPaint, solidRectangle);
            canvas.SetShadow(new SizeF(10,10),10,Colors.Gray);
            canvas.FillRoundedRectangle(solidRectangle, 12);
        }
    }
}
