namespace CanvasUsage
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var vm = BindingContext as ViewModels.MainPageViewModel;
            var drawFigure = vm?.MyDraw as GraphicViews.DrawFigure;
            drawFigure?.RequestRedraw += () => DrawBox.Invalidate(); // Subscribe to the RequestRedraw event to invalidate the canvas when the figure changes
            var drawPaint = vm?.MyPaint as GraphicViews.DrawPaint;
            drawPaint?.RequestRedraw += () => TextureDrawBox.Invalidate(); // Subscribe to the RequestRedraw event to invalidate the canvas when the paint changes
        }

    }
}
