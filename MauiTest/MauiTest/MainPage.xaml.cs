using Microsoft.Maui.Graphics.Platform;
using System.Diagnostics;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace MauiTest
{
    public partial class MainPage : ContentPage
    {
        //int count = 0;

        public MainPage()
        {
            InitializeComponent();
            //this.Loaded += OnPageLoaded;
        }
        string? translatedNumber;

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = PhoneNumberText.Text;
            translatedNumber = PhonewordTranslator.ToNumber(enteredNumber);
            if (!string.IsNullOrEmpty(translatedNumber))
            {
                CallButton.IsEnabled = true;
                CallButton.Text = "Call " + translatedNumber;
            }
            else
            {
                CallButton.IsEnabled = false;
                CallButton.Text = "Call";
            }
        }

        private async void OnCall(object sender, EventArgs e)
        {
            if (await this.DisplayAlert("打电话", "你要打给 " + translatedNumber + "吗？", "是", "否"))
            {
                Debug.WriteLine("嘟嘟嘟...");
                try
                {
                    if (PhoneDialer.Default.IsSupported)
                    {
                        PhoneDialer.Default.Open(translatedNumber!);
                    }
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("无法呼叫", "电话号码格式错误", "Ok");
                }
                catch (Exception)
                {
                    await DisplayAlert("无法呼叫", "呼叫失败", "Ok");
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                //await button.RotateTo(360, 2000, Easing.Linear); //只会旋转一次，因为旋转角度是相对于当前的，所以每次都是从0开始旋转到360
                await button.RelRotateTo(360, 2000, Easing.Linear); //会一直旋转，因为每次都是在当前角度基础上旋转360
                Pic.Opacity = 0;
                _ = Pic.FadeTo(1, 4000); //图片淡入 不等待可同时执行
                await Pic.RelRotateTo(360, 2000, Easing.Linear); //图片也旋转
                await Pic.ScaleTo(1.5, 2000, Easing.BounceIn); //图片
                await Pic.TranslateTo(100, -100, 1000); //图片平移 向右 向上
                //Pic.Opacity = 0;
                //await Pic.FadeTo(1,4000); //图片淡入
            }
        }


        private async void OnPageLoaded(object? sender, EventArgs e)
        {
            using Stream stream = await FileSystem.OpenAppPackageFileAsync("galaxy.png");
            IImage graph = PlatformImage.FromStream(stream);
            if (graph.Width > 0 && graph.Height > 0)
            {
                Debug.WriteLine($"图片加载成功，宽度：{graph.Width}，高度：{graph.Height}");
            }
            else
            {
                Debug.WriteLine("图片加载失败，无法获取尺寸");

            }
        }
    }

}
