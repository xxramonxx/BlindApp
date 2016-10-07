using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace BlindApp
{
    public class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            Master = new MenuPage();
            Detail = new NavigationPage(new SpeechDetailPage());
        }
    }
}
