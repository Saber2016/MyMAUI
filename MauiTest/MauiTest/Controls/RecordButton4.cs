using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Controls
{
    /// <summary>
    /// 对于唱片机播放唱片的按钮，除了旋转外，还需要一个中心孔洞来模拟CD的外观
    /// 对RecordButton3的升级版，主要是增加了中心孔洞，并且把旋转逻辑放在控件内部，外部只需要设置IsRotating属性即可控制旋转
    /// 但是这个空不是镂空的，而是一个覆盖在图片上的圆形，颜色与页面背景色一致，模拟透明效果，这样就不需要担心裁剪导致的性能问题了
    /// </summary>
    public class RecordButton4 :ContentView
    {
        private readonly Image _image;
        private readonly Border _border;
        private readonly Ellipse _centerHole;
        private readonly Grid _container;
        private bool _isRotating;

        #region 可绑定属性
        public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(RecordButton4), default);

        public static readonly BindableProperty IsRotatingProperty =
            BindableProperty.Create(nameof(IsRotating), typeof(bool), typeof(RecordButton4), false,
                propertyChanged: OnIsRotatingChanged);

        public static readonly BindableProperty SizeProperty =
            BindableProperty.Create(nameof(Size), typeof(double), typeof(RecordButton4), 120.0,
                propertyChanged: OnSizeChanged);

        public static readonly BindableProperty RotationDurationProperty =
            BindableProperty.Create(nameof(RotationDuration), typeof(uint), typeof(RecordButton4), 3000u);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RecordButton4), Colors.Transparent);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(RecordButton4), 0.0);

        public static readonly BindableProperty CenterHoleRatioProperty =
            BindableProperty.Create(nameof(CenterHoleRatio), typeof(double), typeof(RecordButton4), 0.20,
                propertyChanged: OnClipPropertyChanged);

        /// <summary>
        /// 中心孔洞颜色，应与页面背景色一致以模拟透明效果。
        /// 默认黑色（CD 中心通常是黑色）。
        /// </summary>
        public static readonly BindableProperty CenterHoleColorProperty =
            BindableProperty.Create(nameof(CenterHoleColor), typeof(Color), typeof(RecordButton3), Colors.Black,
                propertyChanged: OnCenterHoleColorChanged);
        #endregion

        #region 公开属性
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public bool IsRotating
        {
            get => (bool)GetValue(IsRotatingProperty);
            set => SetValue(IsRotatingProperty, value);
        }

        public uint RotationDuration
        {
            get => (uint)GetValue(RotationDurationProperty);
            set => SetValue(RotationDurationProperty, value);
        }

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public double CenterHoleRatio
        {
            get => (double)GetValue(CenterHoleRatioProperty);
            set => SetValue(CenterHoleRatioProperty, value);
        }

        public Color CenterHoleColor
        {
            get => (Color)GetValue(CenterHoleColorProperty);
            set => SetValue(CenterHoleColorProperty, value);
        }
        #endregion
        public event EventHandler? Toggled;

        public RecordButton4()
        {
            // ---- 层1：圆形图片 ----
            _image = new Image
            {
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            _border = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 999 },
                Stroke = BorderColor,
                StrokeThickness = BorderWidth,
                Content = _image,
                WidthRequest = Size,
                HeightRequest = Size,
                Padding = 0,
                // 用整圆裁剪图片，保持圆形外观
                Clip = new EllipseGeometry
                {
                    Center = new Point(Size / 2, Size / 2),
                    RadiusX = Size / 2,
                    RadiusY = Size / 2
                }
            };
            // ---- 层2：中心孔洞（覆盖在图片上方）----
            double holeSize = Size * CenterHoleRatio;
            _centerHole = new Ellipse
            {
                WidthRequest = holeSize,
                HeightRequest = holeSize,
                Fill = CenterHoleColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            // ---- 用 Grid 叠放两层 ----
            _container = new Grid
            {
                WidthRequest = Size,
                HeightRequest = Size,
                Children = { _border, _centerHole }
            };

            // 手势加在容器上，确保点击任何区域都能响应
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnTapped;
            _container.GestureRecognizers.Add(tapGesture);

            Content = _container;

            _image.SetBinding(Image.SourceProperty,
                new Binding(nameof(ImageSource), source: this));
        }

        private void OnTapped(object? sender, TappedEventArgs e)
        {
            IsRotating = !IsRotating;
            Toggled?.Invoke(this, EventArgs.Empty);
        }

        private void StartRotation()
        {
            this.AbortAnimation("VinylRotation");

            double startAngle = _image.Rotation % 360;

            var animation = new Animation(
                callback: v => _image.Rotation = v,
                start: startAngle,
                end: startAngle + 360,
                easing: Easing.Linear);

            animation.Commit(
                owner: this,
                name: "VinylRotation",
                rate: 16,
                length: RotationDuration,
                easing: Easing.Linear,
                finished: (v, c) =>
                {
                    _image.Rotation = startAngle;
                },
                repeat: () => IsRotating);
        }

        private void StopRotation()
        {
            this.AbortAnimation("VinylRotation");
        }

        #region 属性变更回调
        private static void OnIsRotatingChanged(BindableObject bindable,
        object oldValue, object newValue)
        {
            if (bindable is RecordButton4 control && newValue is bool rotating)
            {
                if (rotating)
                    control.StartRotation();
                else
                    control.StopRotation();
            }
        }

        private static void OnSizeChanged(BindableObject bindable,
            object oldValue, object newValue)
        {
            if (bindable is RecordButton4 control && newValue is double size)
            {
                double holeSize = size * control.CenterHoleRatio;

                // 更新容器
                control._container.WidthRequest = size;
                control._container.HeightRequest = size;

                // 更新圆形图片
                control._border.WidthRequest = size;
                control._border.HeightRequest = size;
                control._border.Clip = new EllipseGeometry
                {
                    Center = new Point(size / 2, size / 2),
                    RadiusX = size / 2,
                    RadiusY = size / 2
                };

                // 更新中心孔洞
                control._centerHole.WidthRequest = holeSize;
                control._centerHole.HeightRequest = holeSize;
            }
        }

        private static void OnClipPropertyChanged(BindableObject bindable,
            object oldValue, object newValue)
        {
            if (bindable is RecordButton4 control && newValue is double ratio)
            {
                double holeSize = control.Size * ratio;
                control._centerHole.WidthRequest = holeSize;
                control._centerHole.HeightRequest = holeSize;
            }
        }

        private static void OnCenterHoleColorChanged(BindableObject bindable,
            object oldValue, object newValue)
        {
            if (bindable is RecordButton4 control && newValue is Color color)
            {
                control._centerHole.Fill = color;
            }
        }
        #endregion
    }
}
