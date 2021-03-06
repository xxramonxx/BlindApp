using System;

using Android.App;
using Android.Content;
using BlindApp.Droid;
using Android.Speech;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechRecognition))]

namespace BlindApp.Droid
{
    class SpeechRecognition : ISpeechRecognition
    {
        private bool isListening;
        private Intent recognizerIntent;
        private SpeechRecognizer speech = null;
        private SpeechRecogintionListener listener;

        public void Initialize()
        {
            isListening = false;
            listener = new SpeechRecogintionListener();
            speech = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);
            speech.SetRecognitionListener(listener);

            recognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguagePreference, "sk");
            recognizerIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, Application.Context.PackageName);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelWebSearch);

            recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1000);
         //   recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 500);
            recognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 500);

            //    recognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
        }

        public void Start()
        {
            speech.StartListening(recognizerIntent);
            isListening = true;
        }

        public void Stop()
        {
            speech.StopListening();
            isListening = false;
        }

        public Boolean IsListening()
        {
            return isListening;
        }
    }
}