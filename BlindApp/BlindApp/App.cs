using BlindApp.Interfaces;
using Xamarin.Forms;

namespace BlindApp
{
    public class App : Application
    {
 
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static TextToSpeech Speaker;

        private IBluetoothController Bluetooth;

        public App()
        {
            new Database.Initialize();

            Bluetooth = DependencyService.Get<IBluetoothController>();
            Speaker = new TextToSpeech();

            MainPage = new MainPage();
        }
       
        protected override void OnStart()
        {
            // Handle when your app starts
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
                App.Speaker.speakNext("Bluetooth zapnutý");
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            if (Bluetooth.IsEnabled())
            {
                Bluetooth.Stop();
                App.Speaker.speak("Bluetooth vypnutý");
            }

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (!Bluetooth.IsEnabled())
            {
                Bluetooth.Start();
                App.Speaker.speak("Bluetooth zapnutý");
            }
        }
    }
}
