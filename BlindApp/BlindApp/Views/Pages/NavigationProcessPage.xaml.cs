using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.Diagnostics;

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
			var beaconsAround = Building.Info.Where(item =>
			   	((item.Location.X > position.Location.X - radius
			   	&& item.Location.X < position.Location.X + radius)
			   	&& item.Location.Y > position.Location.Y - radius
			    && item.Location.Y < position.Location.Y + radius))
			    .OrderBy(item => position.Location.Distance(item.Location));
			var point = beaconsAround.FirstOrDefault();
			var finalAngle = -GetAngle(position.Location, point.Location);

			double compassAngle = 0;
			if (_NavigationHandler.Rotation < 180)
				compassAngle = _NavigationHandler.Rotation;
			else 
				compassAngle = _NavigationHandler.Rotation - 360;
				

			var sight = 45;

			var minusAngle = compassAngle - sight;
			var plusAngle = compassAngle + sight;

			if (finalAngle > minusAngle && finalAngle < plusAngle)
			{
				Debug.WriteLine("Pred tebou");
			}
			else if (finalAngle < minusAngle && finalAngle > (minusAngle - 2 * sight))
			{
				Debug.WriteLine("Vlavo");
			}
			else if (finalAngle > plusAngle && finalAngle < (plusAngle + 2 * sight))
			{
				Debug.WriteLine("Vpravo");
			}
			else
			{
				Debug.WriteLine("Za tebou");
			}

			/*
			 * var minusAngle = _NavigationHandler.Rotation - sight;
			var plusAngle = _NavigationHandler.Rotation + sight;*/

			/*var testvpravo = GetAngle(position.Location, new Point
			{
				X = 1186,
				Y = -1836
			});
			
			var test = GetAngle(position.Location, point.Location);
var angle = -GetAngle(position.Location, point.Location) + 360;
			

			Debug.WriteLine("MINUS: "+ minusAngle);
			Debug.WriteLine("PLUS: "+ plusAngle);*/



		/*	if (angle > minusAngle && angle < plusAngle)
			{
				Debug.WriteLine("Pred tebou");
			}
			else if (angle > plusAngle && angle > 240)
			{
				Debug.WriteLine("Vlavo");
			}
			else if (angle > plusAngle && angle < 120)
			{
				Debug.WriteLine("Vpravo");
			}
			else
			{
				Debug.WriteLine("Za tebou");
			}*/

			// ROMAN
			/*if (angle > minusAngle && angle < plusAngle)
			{
				Debug.WriteLine("Pred tebou");
			}
			else if (angle > (minusAngle + 90) && angle < (plusAngle + 90))
			{
				Debug.WriteLine("Vpravo");
			}
			else if (angle > (minusAngle + 180) && angle < (plusAngle + 180))
			{
				Debug.WriteLine("Za tebou");
			}
			else if (angle > (minusAngle + 270) && angle < (plusAngle + 270))
			{
				Debug.WriteLine("Vlavo");
			}*/
		}

         /* Fetches angle relative to screen centre point
		 * where 3 O'Clock is 0 and 12 O'Clock is 270 degrees
		 *
		 * @param screenPoint
		 * @return angle in degress from 0-360.
		 */
		public double GetAngle(Point Source, Point Target)
		{
			//double dx = Source.X - Target.X;
			//// Minus to correct for coord re-mapping
			//double dy = -(Source.X - Target.Y);

			//double inRads = Math.Atan2(dy, dx);

			//// We need to map to coord system when 0 degree is at 3 O'clock, 270 at 12 O'clock
			//if (inRads < 0)
			//	inRads = Math.Abs(inRads);
			//else
			//	inRads = 2 * Math.PI - inRads;

			//return Math.PI * inRads / 180.0;
			var xDiff = Target.X - Source.X;
			var yDiff = Target.Y - Source.Y;

			// Shiftnutie 0 na sever
			return (Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI) - 90;
		}

		//public static double angleOf(PointF p1, PointF p2)
		//{
		//	// NOTE: Remember that most math has the Y axis as positive above the X.
		//	// However, for screens we have Y as positive below. For this reason, 
		//	// the Y values are inverted to get the expected results.
		//	final double deltaY = (p1.y - p2.y);
		//	final double deltaX = (p2.x - p1.x);
		//	final double result = Math.toDegrees(Math.atan2(deltaY, deltaX));
		//	return (result < 0) ? (360d + result) : result;
    }
}
