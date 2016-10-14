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
using Android.Speech;
using Android.Util;
using System.Threading.Tasks;
using Java.Util;
using Java.Lang;

namespace BlindApp.Droid
{
    class SpeechRecogintionListener: Java.Lang.Object, IRecognitionListener
    {
        public IList<string> matches = null;

        public void OnRmsChanged(float rmsdB)
        {
     //       progressBar.Progress = ((int)rmsdB);
        }

        public void OnBeginningOfSpeech()
        {
            Log.Debug("OnBeginningOfSpeech", "log");
        }

        public void OnBufferReceived(byte[] buffer)
        {
            Log.Debug("OnBufferReceived", "error");
        }

        public void OnEndOfSpeech()
        {
            Log.Debug("OnEndOfSpeech", "log");
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            string errorMessage = getErrorText(error);
      //      returnedText.Text = errorMessage;
        }

        public void OnEvent(int eventType, Bundle @params)
        {
            Log.Debug("OnEvent", "error");
        }

        public void OnPartialResults(Bundle partialResults)
        {
            Log.Debug("OnPartialResults", "OnPartialResults");
        }

        public void OnReadyForSpeech(Bundle @params)
        {
            Log.Debug("OnReadyForSpeech", "OnReadyForSpeech");
        }

        public void OnResults(Bundle results)
        {
            new CallAsync(new Callback()).Execute(results.GetStringArrayList(SpeechRecognizer.ResultsRecognition) as Java.Lang.Object);
            /*matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);

            string text = "";
            foreach (string result in matches)
                text += result + "\n";

            returnedText.Text = text;*/
        }

        public static string getErrorText(SpeechRecognizerError errorCode)
        {
            string message;
            switch (errorCode)
            {
                case SpeechRecognizerError.Audio:
                    message = "Audio recording error";
                    break;
                case SpeechRecognizerError.Client:
                    message = "Client side error";
                    break;
                case SpeechRecognizerError.InsufficientPermissions:
                    message = "Insufficient permissions";
                    break;
                case SpeechRecognizerError.Network:
                    message = "Network error";
                    break;
                case SpeechRecognizerError.NetworkTimeout:
                    message = "Network timeout";
                    break;
                case SpeechRecognizerError.NoMatch:
                    message = "No match";
                    break;
                case SpeechRecognizerError.RecognizerBusy:
                    message = "RecognitionService busy";
                    break;
                case SpeechRecognizerError.Server:
                    message = "error from server";
                    break;
                case SpeechRecognizerError.SpeechTimeout:
                    message = "Sspeech timeout";
                    break;
                default:
                    message = "Didn't understand, please try again.";
                    break;
            }
            return message;
        }

        class CallAsync : AsyncTask
        {
            OnTaskCompleted listener;
            IList<string> result;

            public CallAsync(OnTaskCompleted listener)
            {
                this.listener = listener;
            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                Console.WriteLine("DoInBackground", "Start");

                var resultList = @params[0] as IList<string>;

                while (resultList.Count == 0)
                {
                    //wait
                }

                this.result = resultList;

                return null;
            }

            protected override void OnProgressUpdate(params Java.Lang.Object[] values)
            {
                Console.WriteLine("OnProgressUpdate", "Start");
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                Console.WriteLine("OnPostExecute", "Start");

                try
                {
                    listener.onTaskCompleted(this.result);
                } catch (AndroidException e)
                {
                    Console.WriteLine("OnPostExecute", e.StackTrace);
                }
            }
        }
    }
}