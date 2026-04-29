using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;
using System.Windows.Input;

namespace RingImage
{
    // All the code in this file is included in all platforms.
    public class RingImageView : ContentView
    {
        private readonly SKCanvasView _canvasView;
        private SKBitmap? _bitmap;
        private IDispatcherTimer? _timer;
        private DateTime _lastUpdate;
        private float _rotationDegrees;

        public RingImageView()
        {
            _canvasView = new SKCanvasView();
            _canvasView.PaintSurface += OnPaintSurface;
            Content = _canvasView;

            var tap = new TapGestureRecognizer();
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);
        }

        #region 可绑定属性
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(
            nameof(Source), typeof(ImageSource), typeof(RingImageView), null, propertyChanged: OnSourceChanged);
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public async static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RingImageView imageView && newValue is ImageSource source)
            {
                imageView._bitmap?.Dispose();
                imageView._bitmap = null;
                if (imageView.Source == null)
                {
                    imageView._canvasView.InvalidateSurface();
                    return;
                }

                try
                {
                    // 利用 MAUI 的 ImageSource 加载能力，通过临时 Image 控件获取数据
                    var stream = await GetStreamFromImageSourceAsync(imageView.Source);
                    if (stream != null)
                    {
                        using (stream)
                        {
                            imageView._bitmap = SKBitmap.Decode(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[RingImageView] Load failed: {ex}");
                }

                imageView._canvasView.InvalidateSurface();
            }
        }
        private static async Task<Stream?> GetStreamFromImageSourceAsync(ImageSource source)
        {
            try
            {
                // 对于 FileImageSource，先尝试直接用 FileSystem
                if (source is FileImageSource fileSource)
                {
                    var file = fileSource.File;

                    // 尝试作为应用包文件打开（MauiImage 资源）
                    try
                    {
                        return await FileSystem.OpenAppPackageFileAsync(file);
                    }
                    catch { }

                    // 尝试文件系统路径
                    if (System.IO.File.Exists(file))
                    {
                        return new FileStream(file, FileMode.Open, FileAccess.Read);
                    }

                    // 尝试常见的相对路径
                    var searchPaths = new[]
                    {
                Path.Combine(FileSystem.AppDataDirectory, file),
                Path.Combine(FileSystem.CacheDirectory, file),
            };

                    foreach (var path in searchPaths)
                    {
                        if (System.IO.File.Exists(path))
                            return new FileStream(path, FileMode.Open, FileAccess.Read);
                    }
                }

                // 对于 UriImageSource
                if (source is UriImageSource uriSource)
                {
                    using var client = new HttpClient();
                    var bytes = await client.GetByteArrayAsync(uriSource.Uri);
                    return new MemoryStream(bytes);
                }

                // 对于 StreamImageSource
                if (source is StreamImageSource streamSource)
                {
                    return await streamSource.Stream(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RingImageView] GetStream failed: {ex.Message}");
            }

            return null;
        }


        public static readonly BindableProperty IsPlayingProperty =
            BindableProperty.Create(nameof(IsPlaying), typeof(bool), typeof(RingImageView), false, BindingMode.TwoWay, propertyChanged: OnIsPlayingChanged);
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }
        public static void OnIsPlayingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is RingImageView imageView)
            {
                if (imageView.IsPlaying)
                    imageView.StartRotation();
                else
                    imageView.StopRotation();
            }
        }

        /// <summary>旋转速度，单位：度/秒，默认 45</summary>
        public static readonly BindableProperty RotationSpeedProperty = BindableProperty.Create(
        nameof(RotationSpeed), typeof(double), typeof(RingImageView), 45.0);

        public double RotationSpeed
        {
            get => (double)GetValue(RotationSpeedProperty);
            set => SetValue(RotationSpeedProperty, value);
        }

        /// <summary>外圆半径占控件短边的比例，默认 0.48</summary>
        public static readonly BindableProperty OuterRadiusRatioProperty = BindableProperty.Create(
            nameof(OuterRadiusRatio), typeof(double), typeof(RingImageView), 0.48,
            validateValue: (_, v) => (double)v is > 0 and <= 1.0);

        public double OuterRadiusRatio
        {
            get => (double)GetValue(OuterRadiusRatioProperty);
            set => SetValue(OuterRadiusRatioProperty, value);
        }

        /// <summary>内圆镂空半径占控件短边的比例，默认 0.15</summary>
        public static readonly BindableProperty InnerRadiusRatioProperty = BindableProperty.Create(
            nameof(InnerRadiusRatio), typeof(double), typeof(RingImageView), 0.15,
            validateValue: (_, v) => (double)v is >= 0 and < 1.0);

        public double InnerRadiusRatio
        {
            get => (double)GetValue(InnerRadiusRatioProperty);
            set => SetValue(InnerRadiusRatioProperty, value);
        }

        /// <summary>环形边框颜色</summary>
        public static readonly BindableProperty RingColorProperty = BindableProperty.Create(
            nameof(RingColor), typeof(Color), typeof(RingImageView), Colors.Silver);

        public Color RingColor
        {
            get => (Color)GetValue(RingColorProperty);
            set => SetValue(RingColorProperty, value);
        }

        /// <summary>点击命令，参数为当前 IsPlaying 状态</summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(RingImageView), null);

        public ICommand? Command
        {
            get => (ICommand?)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion

        private void OnTapped(object? sender,TappedEventArgs e)
        {
            IsPlaying = !IsPlaying;
            if(Command?.CanExecute(IsPlaying) == true)
            {
                Command?.Execute(IsPlaying);
            }
        }

        private void StartRotation()
        {
            StopRotation();
            _lastUpdate = DateTime.UtcNow;
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMicroseconds(16);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private void StopRotation()
        {
            _timer?.Stop();
            _timer = null;
        }
        
        private void OnTimerTick(object? sender,EventArgs e)
        {
            if (!IsPlaying)
                return;
            var now = DateTime.UtcNow;
            var delta = (now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;
            _rotationDegrees += (float)(RotationSpeed*delta)%360;
            _canvasView.InvalidateSurface();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _canvasView.InvalidateSurface();
        }

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);
            if (args.NewHandler == null)
            {
                // 控件销毁时清理
                StopRotation();
                _bitmap?.Dispose();
                _bitmap= null;
            }
        }

        private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;
            canvas.Clear(SKColors.Transparent);

            if (_bitmap == null)
                return;

            float cx = info.Width / 2f;
            float cy = info.Height / 2f;
            float min = Math.Min(cx, cy);
            float outerR = (float)(OuterRadiusRatio * min);
            float innerR = (float)(InnerRadiusRatio * min);
            canvas.Save();

            // 1. 以中心点为轴旋转画布
            canvas.Translate(cx, cy);
            canvas.RotateDegrees(_rotationDegrees);
            canvas.Translate(-cx, -cy);

            // 2. 构建环形裁剪区（EvenOdd：外圆 - 内圆 = 环形）
            using var clip = new SKPath();
            clip.AddCircle(cx, cy, outerR);
            clip.AddCircle(cx, cy, innerR);
            clip.FillType = SKPathFillType.EvenOdd;
            canvas.ClipPath(clip);

            // 3. 计算图片裁剪：按 AspectFill 方式裁成方形，填满外接圆
            var dest = new SKRect(cx - outerR, cy - outerR, cx + outerR, cy + outerR);
            float srcX, srcY, srcW, srcH;
            float aspect = (float)_bitmap.Width / _bitmap.Height;

            if (aspect > 1f) // 宽图，裁左右
            {
                srcH = _bitmap.Height;
                srcW = srcH;
                srcX = (_bitmap.Width - srcW) / 2f;
                srcY = 0;
            }
            else // 高图，裁上下
            {
                srcW = _bitmap.Width;
                srcH = srcW;
                srcX = 0;
                srcY = (_bitmap.Height - srcH) / 2f;
            }

            var src = new SKRect(srcX, srcY, srcX + srcW, srcY + srcH);

            using var paint = new SKPaint
            {
                IsAntialias = true,
           
            };
            canvas.DrawBitmap(_bitmap, src, dest, paint);

            canvas.Restore();

            // 4. 绘制装饰边框（外圈 + 内圈）
            using var ringPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = RingColor.ToSKColor(),
                StrokeWidth = Math.Max(1, min * 0.008f)
            };

            canvas.DrawCircle(cx, cy, outerR, ringPaint);
            ringPaint.StrokeWidth = Math.Max(1, min * 0.012f);
            canvas.DrawCircle(cx, cy, innerR, ringPaint);
        }
    }

    public static class AppBuilderExtensions
    {
        public static MauiAppBuilder UseRingImageView(this MauiAppBuilder builder)
        {
            return builder;
        }
    }
}
