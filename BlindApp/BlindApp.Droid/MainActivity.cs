using System;

using Android.App;
using Android.Content.PM;
using AltBeaconOrg.BoundBeacon;

using Android.OS;
using Plugin.TextToSpeech;

namespace BlindApp.Droid
{
    [Activity(
        Label = "BlindApp",
        Icon = "@drawable/icon",
      //  MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/MyTheme"
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IBeaconConsumer
    {
        private IAltBeaconService beaconService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossTextToSpeech.Current.Init();
            LoadApplication(new App());
        }
        
        #region IBeaconConsumer Implementation
        public void OnBeaconServiceConnect()
        {
            beaconService = Xamarin.Forms.DependencyService.Get<IAltBeaconService>();

            beaconService.StartMonitoring();
            beaconService.StartRanging();
        }
        #endregion
    }
}

