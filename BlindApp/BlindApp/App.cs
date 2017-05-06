﻿using System;
using BlindApp.Interfaces;
using BlindApp.Model;
using BlindApp.Views.Pages;
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

            Building.Init();

            TextToSpeech.Init();
            SpeechRecognition.Init();

            Bluetooth = DependencyService.Get<IBluetoothController>();

            Compass = DependencyService.Get<ICompass>();
            Compass.Start();

			InteractivityLogger = new InteractivityLogger();
        }
       
        protected override void OnStart()
        {
			if (Bluetooth.IsAdapterInicialized)
            {
                initBTstate = Bluetooth.IsEnabled();
                // Handle when your app starts
                if (!initBTstate)
                {
                    Bluetooth.Start();
                    TextToSpeech.speakNext("Bluetooth zapnutý");
                }
				InteractivityLogger.StartLogging();
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
				InteractivityLogger.StopLogging();
                TextToSpeech.Speak("Bluetooth vypnutý");
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
				InteractivityLogger.StartLogging();
                TextToSpeech.Speak("Bluetooth zapnutý");
            }
        }
    }
}
