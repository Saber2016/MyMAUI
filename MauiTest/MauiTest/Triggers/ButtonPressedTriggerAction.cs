using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Triggers
{
    /// <summary>
    /// 因为拆分成两个触发器，按下和释放，无法将原本的颜色信息传递，所以采用状态判断方式
    /// TriggerAction 的一个重要陷阱：每次触发都会创建新的实例，因此无法在两次调用之间共享状态（如保存原始颜色）。
    /// 通过附加属性在 Button 本身保存原始颜色，绕过 TriggerAction 实例隔离的问题：
    /// ps: 1. 这种方式虽然解决了状态共享问题，但也增加了复杂性和维护成本，因为需要管理附加属性的生命周期和正确性。
    /// 不如采用 Behavior 的方式，直接在 Button 上附加一个 Behavior 来处理按下和释放的动画效果，这样就可以在同一个实例中维护状态了。
    /// 这里只是为了演示 TriggerAction 的使用，才采用了这种方式，实际开发中建议使用 Behavior 来实现这种交互效果。
    /// </summary>
    public class ButtonPressedTriggerAction : TriggerAction<Button>
    {
        //定义一个附加属性来保存原始颜色   
        private static readonly BindableProperty OriginalColorProperty =
            BindableProperty.CreateAttached("OriginalColor", typeof(Color), typeof(ButtonPressedTriggerAction), null);

        private static Color? GetOriginalColor(BindableObject view)
        {
            return (Color?)view.GetValue(OriginalColorProperty);
        }

        private static void SetOriginalColor(BindableObject view, Color? value)
        {
            view.SetValue(OriginalColorProperty, value);
        }

        protected async override void Invoke(Button sender)
        {
            if (sender.IsPressed)
            {                
                await OnPressed(sender);
            }
            else
            {
                await OnReleased(sender);
            }
        }

        private async Task OnPressed(Button sender)
        {
            SetOriginalColor(sender, sender.BackgroundColor);
            //1. 快速缩小模拟按下效果
            await sender.ScaleTo(0.97, 80, Easing.CubicOut);
            //颜色变化产生涟漪感
            var darkColor = sender.BackgroundColor.WithLuminosity(0.75f);
            sender.BackgroundColor = darkColor;
        }

        private async Task OnReleased(Button sender)
        {
            var originalColor = GetOriginalColor(sender);
            if(originalColor != null)
            {
                //2. 快速恢复原状
                await sender.ScaleTo(1.0, 100, Easing.SpringOut);
                sender.BackgroundColor = originalColor;
            } 
        }
    }
}
