using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Behaviors
{
    /// <summary>
    /// MAUI 行为，为 Entry 添加聚焦时的动画效果，当 Entry 获得焦点时，放大并改变背景颜色；失去焦点时，恢复原状
    /// 行为和触发器 都能放在Style中，从而能够为一个页面甚至整个程序控件设置动画效果
    /// </summary>
    public class AnimateBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Focused += OnFocused;
            bindable.Unfocused += OnUnfocused;
            base.OnAttachedTo(bindable);
            // 在这里添加动画逻辑，例如订阅事件或启动动画
        }

        private async void OnFocused(object? sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            await entry!.ScaleTo(1.02, 100, easing: Easing.CubicOut);
            entry!.BackgroundColor = Colors.LightYellow;
        }

        private async void OnUnfocused(object? sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            await entry!.ScaleTo(1.0, 100, easing: Easing.CubicIn);
            entry!.BackgroundColor = Colors.White;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Focused -= OnFocused;
            bindable.Unfocused -= OnUnfocused;
            base.OnDetachingFrom(bindable);
        }
    }
}
