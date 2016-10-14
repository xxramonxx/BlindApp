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
using BlindApp.Droid;
using Android.Speech;
using Android.Util;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechRecognition))]

namespace BlindApp.Droid
{
    class SpeechRecognition : ISpeechRecognition
    {
        private SpeechRecognizer speech = null;
        private Intent recognizerIntent;
        private SpeechRecogintionListener listener;

        public void Initialize()
        {
            listener = new SpeechRecogintionListener();
            speech = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);
            speech.SetRecognitionListener(listener);
            recognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguagePreference, "sk");
            recognizerIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, Application.Context.PackageName);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelWebSearch);

    /*        recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 500);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 500);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);*/

            //    recognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
        }

        public void Start()
        {
            speech.StartListening(recognizerIntent);
        }

        public void Stop()
        {
            speech.StopListening();
        }
    }
}