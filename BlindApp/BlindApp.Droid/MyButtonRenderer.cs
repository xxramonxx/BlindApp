using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using BlindApp.Droid;
using Xamarin.Forms;
using BlindApp;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MyButton), typeof(MyButtonRenderer))]
namespace BlindApp.Droid
{
    class MyButtonRenderer : ButtonRenderer
    {
        readonly GestureDetector detector;
        readonly FancyGestureListener listener;
        public MyButtonRenderer()
        {
            listener = new FancyGestureListener();
            detector = new GestureDetector(this.listener);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<global::Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
        }
    }
}