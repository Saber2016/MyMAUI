using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Controls
{
    /// <summary>
    /// RecordButton 的升级版,主要是采用MAUI自带的动画来实现，代码更精简，且能做到立即停止
    /// 
    /// </summary>
    public class RecordButton2: ImageButton
    {
        public static readonly BindableProperty IsRotatingProperty =
            BindableProperty.Create(nameof(IsRotating), typeof(bool), typeof(RecordButton2), false, propertyChanged: OnIsRotatingChanged);

        public bool IsRotating
        {
            get => (bool)GetValue(IsRotatingProperty);
            set => SetValue(IsRotatingProperty, value);
        }

        public int RoationDuration { get; set; } = 2000; //2秒

        public RecordButton2()
        {
            Clicked += OnClicked;
            // 注释这行，效果也不错
            //SizeChanged += OnSizeChanged;
        }

        public static void OnIsRotatingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RecordButton2;
            bool isRotating = (bool)newValue;
            if (isRotating)
                control?.StartRotationAnimation();
            else
                control?.StopRotationAnimation();
        }

        private void StartRotationAnimation()
        {
            // 使用 ViewExtensions.Animate 创建一个名为 "RecordRotation" 的永久动画
            this.Animate("RecordRotation",
                callback: v => Rotation = v,
                start: Rotation,
                end: Rotation + 360,
                length: (uint)RoationDuration,
                easing: Easing.Linear,
                repeat: () => IsRotating); //返回True 则继续循环
        }

        private void StopRotationAnimation()
        {
            //停止指定动画
            this.AbortAnimation("RecordRotation");
            Rotation = 0;  //也可以不回去，假如要做转盘
        }

        //绑定内部点击事件
        public void OnClicked(object? sender, EventArgs e)
        {
            IsRotating = !IsRotating;
        }

        private void OnSizeChanged(Object? sender, EventArgs e)
        {
            //自动圆角化
            if (Width > 0 && Height > 0)
            {
                CornerRadius = (int)Math.Min(Width, Height) / 2;
            }
        }

        // 清理资源：当控件从视觉树移除时
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            //如果handler 为空，说明控件正在被销毁或者从视觉树中移除
            if (Handler == null)
            {
                this.AbortAnimation("RecordRotation");  //停止动画方法
            }
        }

       
    }
}
