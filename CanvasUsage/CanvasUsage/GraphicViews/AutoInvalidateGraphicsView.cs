using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasUsage.GraphicViews
{
    /// <summary>
    /// 如果不想使用附加属性，也可以直接从GraphicsView派生一个类，并添加依赖项属性来绑定绘制数据，这样可以进一步简化代码并提高可重用性。
    /// </summary>
    public class AutoInvalidateGraphicsView:GraphicsView
    {
        public static readonly BindableProperty InvalidateTriggerProperty = BindableProperty.Create(
            "InvalidateTrigger",
            typeof(object),
            typeof(AutoInvalidateGraphicsView),
            null,
            propertyChanged: (bindableObject,oldValue,newValue)=>(bindableObject as GraphicsView)?.Invalidate());

        public object InvalidateTrigger
        {
            get => GetValue(InvalidateTriggerProperty);
            set => SetValue(InvalidateTriggerProperty, value);
        }
    }
}
