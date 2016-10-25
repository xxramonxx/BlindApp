using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BlindApp
{
    public class App : Application
    {
 
        public static int ScreenWidth;
        public static int ScreenHeight;

        public App()
        {
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts aa
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
