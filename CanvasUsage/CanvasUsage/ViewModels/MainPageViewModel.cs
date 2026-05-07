using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanvasUsage.GraphicViews;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CanvasUsage.ViewModels
{
    public partial class MainPageViewModel:ObservableObject
    {
        public IDrawable MyDraw { get; set; } = new DrawFigure();
        public IDrawable MyPaint { get; set; } = new DrawPaint();
        public static List<string> Figures { get; } = new List<string>
        {
            "Line",
            "DashLine",
            "Ellipse",
            "Circle",
            "FillEllipse",
            "FillCircle",
            "Rectangle",
            "FillRectangle",
            "RoundRectangle",
            "FillRoundRectangle",
            "Arc",
            "Arc2",
            "FillArc",
            "Path",
            "Triangle",
            "FillTriangle",
            "Image",
            "String",
            "AttributeText",
            "Example1",
            "Example2",
            "LineCap",
            "ClipPath",
            "ClipPath2"
        };
        public static List<string> Paints { get; } = new List<string>
        {
            "SolidBrush",
            "ImagePaint",
            "PatternPaint",
            "LinearGradientPaint",
            "HorizentalLinearGradientPaint",
            "RadialGradientPaint"
        };

        [ObservableProperty]
        public partial string SelectedFigure { get; set; }
        [ObservableProperty]
        public partial string SelectedPaint { get; set; }

        partial void OnSelectedFigureChanged(string value)
        {
            if(MyDraw is DrawFigure drawFigure)
            {
                drawFigure.CurrentFigure = value;               
            }
            
        }

        partial void OnSelectedPaintChanged(string value)
        {
            if(MyPaint is DrawPaint drawPaint)
            {
                drawPaint.SelectedPaint = value;
            }
        }
    }
}
