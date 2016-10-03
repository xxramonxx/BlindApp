using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace BlindApp
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            var speak = new Button
            {
                Text = "Hello, Forms !",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            speak.Clicked += (sender, e) => {
                       DependencyService.Get<ITextToSpeech>().Speak("Hello from Xamarin Forms");
                Debug.WriteLine("helo");
            };
            Content = speak;
        }
    }
}
