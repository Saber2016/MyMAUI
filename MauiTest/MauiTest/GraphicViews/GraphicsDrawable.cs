using Microsoft.Maui.Graphics.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace MauiTest.GraphicViews
{
    public class GraphicsDrawable : IDrawable
    {
        private IImage? _image;
        private bool _isLoading;

        public async Task LoadImageAsync()
        {
            if (_isLoading || _image != null) return;
            _isLoading = true;
            try
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync("galaxy.png");
                _image = PlatformImage.FromStream(stream);
                // 模拟异步加载图片
                //await Task.Delay(2000);
                // 这里应该是实际的图片加载逻辑，例如使用 HttpClient 下载图片并创建 IImage 对象
                // _image = await LoadImageFromUrlAsync(imageUrl);
            }
            finally
            {
                _isLoading = false;
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_image == null)
                return;
            PathF path = new PathF();
            path.AppendCircle(64,64,20);
            path.AppendCircle(64, 64, 64);
            canvas.ClipPath(path,windingMode:WindingMode.EvenOdd);
            canvas.DrawImage(_image, 0,0,_image.Width,_image.Height);
        }
    }
}
