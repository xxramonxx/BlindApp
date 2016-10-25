using System;

using Android.App;
using Android.Content.PM;
using AltBeaconOrg.BoundBeacon;

using Android.OS;
using Plugin.TextToSpeech;
using ScnViewGestures.Plugin.Forms.Droid.Renderers;
using Android.Util;

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

            App.ScreenWidth = (int)Resources.DisplayMetrics.WidthPixels; // real pixels
            App.ScreenHeight = (int)Resources.DisplayMetrics.HeightPixels; // real pixels

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ViewGesturesRenderer.Init();
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

