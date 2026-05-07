using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasUsage.Extensions
{
    /// <summary>
    /// 此方法可以避免在MainPage 中手动绑定事件，具体可以对比两个GraphicsView
    /// </summary>
    public static class GraphicsViewExtensions
    {
        public static readonly BindableProperty InvalidateTriggerProperty = BindableProperty.CreateAttached(
            "InvalidateTrigger",
            typeof(object),
            typeof(GraphicsViewExtensions),
            null,
            propertyChanged: OnInvalidateTriggerChanged);

        private static void OnInvalidateTriggerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is GraphicsView graphicsView)
            {
                graphicsView.Invalidate();  //只要绑定的值发生变化，就调用Invalidate方法刷新GraphicsView
            }
        }

        public static void SetInvalidateTrigger(BindableObject view, object value)
        {
            view.SetValue(InvalidateTriggerProperty, value);
        }
        public static object GetInavlidateTrigger(BindableObject view)
        {
            return view.GetValue(InvalidateTriggerProperty);
        }
    }
}
