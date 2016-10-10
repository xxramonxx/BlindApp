using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace BlindApp.Droid
{
    [Activity(
        Theme = "@style/MyTheme.Splash",
        MainLauncher = true,
        NoHistory = true
        )]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SplashLayout);
            System.Threading.ThreadPool.QueueUserWorkItem(o => LoadActivity());

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void LoadActivity()
        {
            System.Threading.Thread.Sleep(5000); // Simulate a long pause    
            RunOnUiThread(() => StartActivity(typeof(MainActivity)));
        }
    }
}