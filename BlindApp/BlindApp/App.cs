using BlindApp.Interfaces;
using Xamarin.Forms;

namespace BlindApp
{
    public class App : Application
    {
 
        public static int ScreenWidth;
        public static int ScreenHeight;

        private IBluetoothController Bluetooth;

        public App()
        {
            new Database.Initialize();
            new TextToSpeech();

            Bluetooth = DependencyService.Get<IBluetoothController>();

            MainPage = new MainPage();
        }
       
        protected override void OnStart()
        {
            // Handle when your app starts
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
                TextToSpeech.speakNext("Bluetooth zapnutý");
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            if (Bluetooth.IsEnabled())
            {
                Bluetooth.Stop();
                TextToSpeech.speak("Bluetooth vypnutý");
            }

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
                TextToSpeech.speak("Bluetooth zapnutý");
            }
        }
    }
}
