using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public partial class NavigationProcessPage : ContentPage
    {
		NavigationHandler _NavigationHandler = new NavigationHandler();
        public NavigationProcessPage(Target Target)
        {
            InitializeComponent();
            Title = "Navigácia";

            _NavigationHandler.Find(Target);
            BindingContext = _NavigationHandler;

            SpeechRecognition.SetContext(this);

        }

        private void OnTouchDown(object sender, EventArgs args)
        {
            SpeechRecognition.Start();
        }

        private void OnTouchUp(object sender, EventArgs args)
        {
			// SpeechRecognition.Stop();

			var radius = 1000;
			var position = _NavigationHandler.beaconsHandler.Position;
			var beaconsAround = Building.Beacons.Where(item =>
			   	((item.Location.X > position.Location.X - radius
			   	&& item.Location.X < position.Location.X + radius)
			   	&& item.Location.Y > position.Location.Y - radius
			    && item.Location.Y < position.Location.Y + radius))
			    .OrderBy(item => position.Location.Distance(item.Location));
        }
    }
}
