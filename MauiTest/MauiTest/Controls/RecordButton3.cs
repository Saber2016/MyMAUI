using Microsoft.Extensions.Options;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Controls
{
    /// <summary>
    /// 此版本通过继承ContentView 来自定义控件，使用手势点击来触发旋转
    /// 遗憾的是，尝试多次，没能实现一个环形界面
    /// </summary>
    public class RecordButton3 : ContentView
    {
        private readonly Image _image;
        private readonly Border _border; //保存border 引用
        private bool _isRotating;

        #region 可绑定属性
        public static readonly BindableProperty ImageSourceProperty = 
            BindableProperty.Create(nameof(ImageSource),typeof(ImageSource),typeof (RecordButton3),default);

        public static readonly BindableProperty IsRotatingProperty =
            BindableProperty.Create(nameof(IsRotating),typeof(bool),typeof (RecordButton3),false,propertyChanged:OnIsRotatingChanged);

        public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(
            nameof(Size),
            typeof(double),
            typeof(RecordButton3),
            120.0,
            propertyChanged: OnSizeChanged);

        public static readonly BindableProperty RotationDurationProperty =
            BindableProperty.Create(nameof(RotationDuration), typeof(uint), typeof(RecordButton3), 3000u);

        public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(RecordButton3),
            Colors.Transparent);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(
                nameof(BorderWidth),
                typeof(double),
                typeof(RecordButton3),
                0.0);

        public static readonly BindableProperty CenterHoleRatioProperty =
            BindableProperty.Create(nameof(CenterHoleRatio), typeof(double), typeof(RecordButton3), 0.20, propertyChanged: OnClipPropertyChanged);
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
            set => SetValue (IsRotatingProperty, value);
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
        #endregion
        //事件=========
        public event EventHandler? Toggled;

        public RecordButton3()
        {
            //构建视觉树
            _image = new Image
            {
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            _border = new Border
            {
                //StrokeShape = new RoundRectangle { CornerRadius = 999 },
                Stroke = BorderColor,
                StrokeThickness = BorderWidth,
                Content = _image,
                WidthRequest = Size,
                HeightRequest = Size,
                Padding = 0,
                //Clip = CreateRingClip(Size,CenterHoleRatio)
                //Clip = new EllipseGeometry
                //{
                //    Center = new Point(Size / 2, Size / 2),
                //    RadiusX = Size / 2,
                //    RadiusY = Size / 2
                //}
            };
            UpdateClip();

            // 使用 TapGestureRecognizer 而非 Button，避免按钮样式干扰
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnTapped;
            _border.GestureRecognizers.Add(tapGesture);

            Content = _border;

            // 绑定内部属性
            _image.SetBinding(Image.SourceProperty,
                new Binding(nameof(ImageSource), source: this));
        }

        /// <summary>
        /// 创建环形裁剪几何体：外圆 - 内圆 = 环形可见区域
        /// EvenOdd 规则下，两个同心圆之间的环形区域为"可见"，
        /// 内圆覆盖的区域为"奇数填充"从而被镂空。
        /// </summary>
        private static Geometry CreateRingClip(double size, double holeRatio)
        {
            double outerRadius = size / 2.0;
            double innerRadius = (size * holeRatio) / 2.0;
            Point center = new Point(outerRadius, outerRadius);

            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure
            {
                StartPoint = new Point(center.X + outerRadius, center.Y),
                IsClosed = true,
                IsFilled = true
            };

            // 外圆（顺时针）
            pathFigure.Segments.Add(new ArcSegment
            {
                Point = new Point(center.X + outerRadius, center.Y),
                Size = new Size(outerRadius, outerRadius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = true
            });

            // 内圆（逆时针）实现镂空
            if (holeRatio > 0)
            {
                pathFigure.Segments.Add(new ArcSegment
                {
                    Point = new Point(center.X + innerRadius, center.Y),
                    Size = new Size(innerRadius, innerRadius),
                    SweepDirection = SweepDirection.CounterClockwise,
                    IsLargeArc = true
                });
            }

            pathGeometry.Figures.Add(pathFigure);
            pathGeometry.FillRule = FillRule.EvenOdd;

            return pathGeometry;
        }
        private void UpdateClip()
        {
            //if (_border != null)
            //{
            //    _border.Clip = CreateRingClip(Size, CenterHoleRatio);
            //}
            //if (_image != null)
            //{
            //    _image.WidthRequest = Size;
            //    _image.HeightRequest = Size;
            //    _image.Clip = CreateRingClip(Size, CenterHoleRatio);
            //    //_image.Clip = new EllipseGeometry { Center = new Point(Size / 2, Size / 2), RadiusX = Size / 2 * CenterHoleRatio, RadiusY = Size / 2 * CenterHoleRatio };
            //}
            if (_image != null)
            {
                double w = this.WidthRequest; // 或者使用 this.Width，确保获取到正确尺寸
                double h = this.HeightRequest;
                if (w <= 0 || h <= 0) return;

                double cx = w / 2;
                double cy = h / 2;
                double outerR = Math.Min(cx, cy);
                double innerR = outerR * CenterHoleRatio;

                var pathGeometry = new PathGeometry();
                var pathFigure = new PathFigure { StartPoint = new Point(cx + outerR, cy), IsClosed = true };

                // 1. 添加外圆 (顺时针)
                pathFigure.Segments.Add(new ArcSegment
                {
                    Point = new Point(cx + outerR, cy),
                    Size = new Size(outerR, outerR),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = true
                });

                // 2. 仅当孔洞存在时，添加内圆 (逆时针，实现镂空)
                if (innerR > 0)
                {
                    pathFigure.Segments.Add(new ArcSegment
                    {
                        Point = new Point(cx + innerR, cy),
                        Size = new Size(innerR, innerR),
                        SweepDirection = SweepDirection.CounterClockwise,
                        IsLargeArc = true
                    });
                }

                pathGeometry.Figures.Add(pathFigure);
                // 设置 EvenOdd 填充规则，确保内圆部分被镂空
                pathGeometry.FillRule = FillRule.EvenOdd;

                // 将 Clip 应用到 _image 或 this (ContentView) 上
                _image.Clip = pathGeometry;
            }

        }


        private void OnTapped(object? sender, TappedEventArgs e)
        {
            IsRotating = !IsRotating;
            Toggled?.Invoke(this, EventArgs.Empty);
        }

        #region 旋转动画
        private void StartRotation()
        {
            // 先中止可能残留的动画
            this.AbortAnimation("VinylRotation"); 

            // 从当前角度开始，转一圈（360度）
            double startAngle = _image.Rotation % 360;

            var animation = new Animation(
                callback: v => _image.Rotation = v,
                start: startAngle,
                end: startAngle + 360,
                easing: Easing.Linear);

            animation.Commit(
                owner: this,
                name: "VinylRotation",
                rate: 16,                        // ~60fps
                length: RotationDuration,
                easing: Easing.Linear,
                finished: (v, c) =>
                {
                    // 一圈结束后，如果仍在旋转则继续
                    _image.Rotation = startAngle; // 重置避免数值溢出
                },
                repeat: () => IsRotating);      // 返回true则继续循环  Animation.Commit 的 repeat
                                                 // 委托只在动画第一次完成时被调用一次来决定是否重复，而不是每次动画完成时都重新评估！
        }

        private void StopRotation()
        {
            this.AbortAnimation("VinylRotation");
            // 不调用 AbortAnimation，让当前这一圈优雅地转完
            //_isRotating = false; // repeat 返回 false，当前圈结束后自然停止

        }
        #endregion

        #region 属性变更回调
        private static void OnIsRotatingChanged(BindableObject bindable,
        object oldValue, object newValue)
        {
            if (bindable is RecordButton3 control && newValue is bool rotating)
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
            if (bindable is RecordButton3 control && newValue is double size)
            {
                // 更新裁剪区域

                    control._border.WidthRequest = size;
                    control._border.HeightRequest = size;
                    control.UpdateClip();
                
            }
        }

        /// <summary>
        /// CenterHoleRatio 变化时重新生成裁剪
        /// </summary>
        private static void OnClipPropertyChanged(BindableObject bindable,
            object oldValue, object newValue)
        {
            if (bindable is RecordButton3 control )
            {
                control.UpdateClip();
            }
        }
        private static Border? FindBorder(IView? view)
        {
            return view as Border;
        }
        #endregion

    }
}
