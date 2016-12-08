using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BlindApp
{
    public static class TextToSpeech
    {
        static String language;
        static CrossLocale locale;
        static IEnumerable<CrossLocale> locales;

        public static void Init()
        {

            language = "sk";
            locales = CrossTextToSpeech.Current.GetInstalledLanguages();

            if (Device.OS == TargetPlatform.Android)
            {
                locale = locales.FirstOrDefault(l => l.ToString().Substring(0,2).ToLower() == language);
            }
            else
            {
                locale = new CrossLocale { Language = language };//fine for iOS/WP
            }
        }

        public static void Speak(String text)
        {
            CrossTextToSpeech.Current.Speak( text,
            //  pitch: (float)sliderPitch.Value,
            // speakRate: (float)sliderRate.Value,
            //volume: (float)sliderVolume.Value,
                crossLocale: locale
            );
        }

        public static void speakNext(String text)
        {
            CrossTextToSpeech.Current.Speak(
                text,
                true,
            //  pitch: (float)sliderPitch.Value,
            // speakRate: (float)sliderRate.Value,
            //volume: (float)sliderVolume.Value,
                crossLocale: locale
            );
        }
    }
}
