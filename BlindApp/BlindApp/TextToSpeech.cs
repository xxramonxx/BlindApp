using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlindApp
{
    public class TextToSpeech
    {
        String language;
        CrossLocale locale;
        IEnumerable<CrossLocale> locales;

        public TextToSpeech()
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

        public void speak(String text)
        {
            CrossTextToSpeech.Current.Speak( text,
            //  pitch: (float)sliderPitch.Value,
            // speakRate: (float)sliderRate.Value,
            //volume: (float)sliderVolume.Value,
                crossLocale: locale
            );
        }

        public void speakNext(String text)
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
