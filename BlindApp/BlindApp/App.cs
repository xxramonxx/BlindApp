using System;
using System.Collections.Generic;
using BlindApp.Interfaces;
using BlindApp.Model;
using BlindApp.Views.Pages;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BlindApp
{
    public class App : Application
    {

        public static bool DEBUG = true;
 
        public static int ScreenWidth;
        public static int ScreenHeight;

		public static InteractivityLogger InteractivityLogger;
        public static ICompass Compass;

        private IBluetoothController Bluetooth;

        private bool initBTstate;

        public App()
        {
            MainPage = new MainPage();

            TextToSpeech.Init();
            SpeechRecognition.Init();

            Bluetooth = DependencyService.Get<IBluetoothController>();

            Compass = DependencyService.Get<ICompass>();
            Compass.Start();

			InteractivityLogger = new InteractivityLogger();
        }

		protected override void OnStart()
		{
			base.OnStart();

			if (Bluetooth.IsAdapterInicialized)
            {
                initBTstate = Bluetooth.IsEnabled();
                // Handle when your app starts
                if (!initBTstate)
                {
                    Bluetooth.Start();
                    TextToSpeech.speakNext("Bluetooth zapnutý");
                }
            }
		}

		protected override void OnSleep()
		{
			base.OnSleep();

			Compass.Stop();

            // Handle when your app sleeps
            if (Bluetooth.IsEnabled() && initBTstate == false)
            {
                Bluetooth.Stop();
                TextToSpeech.Speak("Bluetooth vypnutý");
            }
		}

		protected override void OnResume()
		{
			base.OnResume();

			InteractivityLogger.SaveCollectedData();

			// Handle when your app resumes
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
                TextToSpeech.Speak("Bluetooth zapnutý");
            }

			//Debug
			var files = DependencyService.Get<IFiles>();

			var oldInteractivityJson = files.LoadFile("interactivity.json");
			System.Diagnostics.Debug.WriteLine(oldInteractivityJson);
		}
    }
}
