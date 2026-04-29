using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MauiTest.Controls
{
    /// <summary>
    /// 一个会旋转的圆形按钮，类似于唱片机播放唱片
    /// 由于使用的是手动循环旋转360,因为动画在旋转时，点击按钮不会立刻停止 await，
    /// 需要等当前周期执行完触发break,所以视觉上不会立即停止
    /// 要立刻停止需要立即停止动画调用 CancelAnimations(),可以参考RecordButton2
    /// </summary>
    public class RecordButton : ImageButton
    {
        private CancellationTokenSource? _cts;

        public bool IsSpinning { get;private set; }

        public RecordButton()
        {
            Clicked += OnClicked;
            //可选:让按钮自动变成圆形
            SizeChanged += OnSizeChanged;
        }

        //公开的IsPlaying属性，供开发者使用
        public static readonly BindableProperty IsPlayingProperty =
            BindableProperty.Create(nameof(IsPlaying), typeof(bool), typeof(RecordButton), false, propertyChanged: OnIsPlayingChanged);

        //如果需要外部控制，可以双向绑定
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        private static void OnIsPlayingChanged(BindableObject bindable,object oldValue,object newValue)
        {
            var button = (RecordButton)bindable;
            var isplaying = (bool)newValue;
            if(isplaying)
            {
                button.StartSpin();
            }
            else
            {
                button?.StopSpin();
            }
        }
        /// <summary>
        /// 此代码是入侵式的，需要用户在按钮点击事件中自动调用：
        /// </summary>
        // MainPage.xaml.cs
        //private void OnRecordButtonClicked(object sender, EventArgs e)
        //{
        //    // 直接切换旋转状态
        //    (sender as RecordButton)?.ToggleSpin();
        //}
        public void TogggleSpin()
        {
            if(IsSpinning)
            {
                StopSpin();
            }
            else
            {
                StartSpin();
            }
        }

        private async void StartSpin()
        {
            if (IsSpinning)
                return;
            IsSpinning = true;
            _cts = new CancellationTokenSource();
            try
            {
                while(true)
                {
                    await this.RotateTo(this.Rotation + 360, 2000, Easing.Linear);
                    if (_cts.IsCancellationRequested)
                        break;
                }
            }
            catch(TaskCanceledException) { 
                //正常操作,不做处理
                }
            finally
            {
                //确保动画结束后旋转角度归零,以便下次启动时位置正确
                this.Rotation = 0;
                IsSpinning = false;
                _cts?.Dispose();
                _cts = null;
            }
                
        }
        private void StopSpin()
        {
            if (!IsSpinning)
                return;
            // 取消当前的旋转动画任务
            _cts?.Cancel();
            // 注意：动画取消后，最终会在 StartSpin 的 finally 中将 Rotation 归零
        }

        private void OnClicked(object? sender, EventArgs e)
        {
            //点击时 自动切换播放状态
            IsPlaying = !IsPlaying;
        }
        private void OnSizeChanged(Object? sender,EventArgs e)
        {
            //自动圆角化
            if(Width>0 && Height>0)
            {
                CornerRadius = (int)Math.Min(Width, Height)/2;
            }
        }

        // 清理资源：当控件从视觉树移除时
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            //如果handler 为空，说明控件正在被销毁或者从视觉树中移除
            if(Handler == null)
            {
                StopSpin() ;  //停止动画方法
            }
        }
    }
}
