using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using BlindApp;
using BlindApp.Droid;
using Android.Views;

[assembly: ExportRenderer(typeof(MyButton), typeof(FancyAndroidButtonRenderer))]

namespace BlindApp.Droid
{
    class FancyAndroidButtonRenderer : ButtonRenderer
    {
        private readonly FancyGestureListener _listener;
        private readonly GestureDetector _detector;

        public FancyAndroidButtonRenderer()
        {
            _listener = new FancyGestureListener();
            _detector = new GestureDetector(_listener);

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                this.GenericMotion -= HandleGenericMotion;
                this.Touch -= HandleTouch;
            }

            if (e.OldElement == null)
            {
                this.GenericMotion += HandleGenericMotion;
                this.Touch += HandleTouch;
            }
        }

        void HandleTouch(object sender, TouchEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }

        void HandleGenericMotion(object sender, GenericMotionEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }
    }
}