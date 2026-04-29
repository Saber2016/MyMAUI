using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Behaviors
{
    /// <summary>
    /// 一个附加属性，用于在任何视图上附加点击事件的行为，
    /// 类似于Xamarin.Forms中的EventToCommandBehavior，但更简单，直接在XAML中设置AttachBehavior为true即可
    /// </summary>
    public static class AttatchedTapBehavior
    {
        public static readonly BindableProperty AttachBehaviorProperty =
            BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(AttatchedTapBehavior), false, propertyChanged: OnAttachedBehaviorChanged);

        public static bool GetAttachBehavior(BindableObject view)
        {
            return (bool)view.GetValue(AttachBehaviorProperty);
        }

        public static void SetAttachBehavior(BindableObject view, bool value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }

        /// <summary>
        /// 暂时设定为为Image添加点击事件，当然也可以扩展到其他视图，甚至是ContentView等容器视图，这样就可以在整个区域内响应点击事件了
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnAttachedBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Image image)
            {
                bool attach = (bool)newValue;
                if (attach)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += OnTapped;
                    image.GestureRecognizers.Add(tapGesture);
                }
                else
                {
                    // 如果不需要附加行为，可以选择移除之前添加的手势识别器
                    var tapGestures = image.GestureRecognizers.OfType<TapGestureRecognizer>().ToList();
                    foreach (var tap in tapGestures)
                    {
                        image.GestureRecognizers.Remove(tap);
                    }
                }
            }
        }

        public static async void OnTapped(object? sender, EventArgs e)
        {
            // 这里可以处理点击事件，例如执行命令或者其他逻辑
            Debug.WriteLine("View was tapped!");
            if(sender is Image image)
            {
                // 可以通过绑定上下文获取命令并执行
                image.Opacity = 0;
                image.Scale = 0.5;
                await Task.WhenAll(image.FadeTo(1, 500,Easing.CubicInOut), image.ScaleTo(1, 500, Easing.SpringOut));
            }

        }
    }
}
