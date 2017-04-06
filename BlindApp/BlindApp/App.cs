using BlindApp.Interfaces;
using BlindApp.Model;
using BlindApp.Views.Pages;
using Xamarin.Forms;

namespace BlindApp
{
    public class App : Application
    {
 
        public static int ScreenWidth;
        public static int ScreenHeight;

        public static BeaconsHandler BeaconsHandler;

        private IBluetoothController Bluetooth;
        private bool initBTstate;

        public App()
        {
            Database.Initializer.Execute();

            TextToSpeech.Init();
            SpeechRecognition.Init();

            Bluetooth = DependencyService.Get<IBluetoothController>();

            BeaconsHandler = new BeaconsHandler();

            MainPage = new MainPage();


            Building.Init();
        }
       
        protected override void OnStart()
        {
            if (Bluetooth.GetAdapter() != null)
            {
                initBTstate = Bluetooth.IsEnabled();
                // Handle when your app starts
                if (!initBTstate)
                {
                    Bluetooth.Start();
                    TextToSpeech.speakNext("Bluetooth zapnutý");
                }
                BeaconsHandler.Init();
            }
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            NavigationHandler.Position.Compass.Stop();

            // Handle when your app sleeps
            if (Bluetooth.IsEnabled() && initBTstate == false)
            {
                Bluetooth.Stop();
                TextToSpeech.Speak("Bluetooth vypnutý");
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
                TextToSpeech.Speak("Bluetooth zapnutý");
            }
            BeaconsHandler.Init();
        }
    }
}
