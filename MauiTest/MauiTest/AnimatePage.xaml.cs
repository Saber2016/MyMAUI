using CommunityToolkit.Maui;

namespace MauiTest;

public partial class AnimatePage : ContentPage
{
	public AnimatePage()
	{
		InitializeComponent();
        this.Loaded += OnPageLoaded;
	}


	private async void OnPageLoaded(object? sender, EventArgs e)
	{
        // 1. 脉冲吸引注意
        await Slider.ScaleTo(1.05, 100, Easing.CubicOut);
        await Slider.ScaleTo(0.98, 80, Easing.CubicIn);
        await Slider.ScaleTo(1, 120, Easing.SpringOut);

        // 2. 数值渐变
        var startValue = Slider.Value;
        var animation = new Animation(v => Slider.Value = v, startValue, 100, Easing.CubicOut);
        animation.Commit(Slider, "ValueAnim", length: 500);

        await Task.Delay(500);

        // 3. 完成反馈

        //await Slider.ScaleTo(1.03, 80, Easing.CubicOut);
        //await Slider.ScaleTo(1, 100, Easing.SpringOut);
        var parentAnimation = new Animation();
        var scaleAnimation = new Animation(v => Slider.Scale = v, Slider.Scale, 1.03, Easing.CubicOut);
        var scaleBackAnimation = new Animation(v => Slider.Scale = v, 1.03, 1, Easing.SpringOut);
        var valueAnimation = new Animation(v => Slider.Value = v, Slider.Value, startValue, Easing.CubicIn);
        parentAnimation.Add(0, 0.5, scaleAnimation);
        parentAnimation.Add(0, 1, valueAnimation);
        parentAnimation.Add(0.5, 1, scaleBackAnimation);
        parentAnimation.Commit(Slider, "CompleteAnim", length: 500);
    }
}