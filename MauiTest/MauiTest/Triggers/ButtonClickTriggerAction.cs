using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui;

namespace MauiTest.Triggers
{
    public class ButtonClickTriggerAction : TriggerAction<Button>
    {

        protected override async void Invoke(Button sender)
        {
            await sender.ScaleTo(0.95, 100, Easing.CubicOut);
            await sender.FadeTo(0.8, 50);
            await Task.WhenAll(
                sender.ScaleTo(1.0, 100, Easing.SpringOut),
                sender.FadeTo(1.0, 100)
            );


        }
    }
}
