using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PropertyChanged;
using BlindApp.Interfaces;

namespace BlindApp
{
    [ImplementPropertyChanged]
    public class NavigationHandler
	{
		public BeaconsHandler beaconsHandler { get; set; } = new BeaconsHandler();
	  	public List<MapPoint> Kokot { get; set; }

		public MapPoint NextMilestone { get; set; }
        
		public  double RemainingMetersNextMilestone;
        public  long RemainingStepsNextMilestone
        {
            get { return Convert.ToInt32(RemainingMetersNextMilestone / STEP_METERS); }
        }

        public long RemainingSteps
        {
            get { return Convert.ToInt32(RemainingMeters / STEP_METERS); }
        }
        public  double RemainingMeters;

        public  bool DestinationReached;

        public double Rotation { get; set; }
        DateTime LastDirectionUpdate;

        private const double STEP_METERS = 1.2f;

        public NavigationHandler()
        {
            RemainingMeters = 0;
            DestinationReached = false;

			beaconsHandler.Init();

            App.Compass.CompassChanged += (s, e) =>
            {
                if (LastDirectionUpdate.AddSeconds(1).CompareTo(DateTime.Now) < 0)
                {
                        // get  vector from  last position and compass change
                        //                var change = new Vec2(1,1);
                 
                    LastDirectionUpdate = DateTime.Now;
                }
                Rotation = e.Values;
            };
        }

		public void Find(Target target)
		{
			RemainingMeters = 0;
			DestinationReached = false;

			//if (App.DEBUG)
			//{

			//	beaconsHandler.Position.XCoordinate = 1100;
			//	beaconsHandler.Position.YCoordinate = -2100;
			//}

			Kokot = NewFind(beaconsHandler.Position.Location, target.Location);
			
            // HACK target ako beacon
			Kokot.Add(new MapPoint
            {
                XCoordinate = target.Location.X,
                YCoordinate = target.Location.Y
            });

            if (Kokot.Count == 0) return;

			//Kokot = path.Clone();
            RemainingMeters = GetKokotDistance();

            StartNavigation();
        }

		private void StartNavigation()
		{
			var thread = DependencyService.Get<IThreadManager>();

			thread.ThreadDelegate += delegate
			{
				var kokotCounter = 0;
				NextMilestone = Kokot.First();
				

				Device.StartTimer(TimeSpan.FromSeconds(1), delegate
				{
					var distanceToNextPoint = NextMilestone.Location.Distance(beaconsHandler.Position.Location);

					if (distanceToNextPoint < 200)
						NextMilestone = Kokot[kokotCounter++];
					
					App.InteractivityLogger.NewRecord(new InteractivityRecord
					{
						Interaction = EInteractionType.AUTOMATIC,
						X = beaconsHandler.Position.XCoordinate,
						Y = beaconsHandler.Position.YCoordinate
					});
					return true;
				});
			};

			thread.Start(thread.CreateNewThread());
		}

        private  double GetKokotDistance()
        {
			var _kokot = Kokot.Clone();
            double distance = 0;

            Point current;
			var next = _kokot.Last().Location;
			_kokot.Remove(Kokot.Last());
			distance += beaconsHandler.Position.Location.Distance(next);

            current = next;
			_kokot.Reverse();

			foreach (var zlokot in _kokot)
			{
				distance += zlokot.Location.Distance(current);
				current = zlokot.Location;
			}
    //        while(Kokot.Count > 0)
    //        {
				//distance += Kokot.Last().Location.Distance(current);
				//current = Kokot.Last().Location;
				//Kokot.Remove(Kokot.Last());
    //        }

            return distance / 100;
        }

        public List<MapPoint> NewFind(Point start, Point target)
        {
            var path = new List<MapPoint>();

            var beacons = Building.Beacons;
            var nearestBeacon = GetCurrnetNearestBeacon(target);
			path.Add(nearestBeacon);

            SharedBeacon targetBeacon = null;
            List<string> visited = new List<string>();

            var limit = 0;

            while (targetBeacon == null && limit < 1000)
            {
                visited.Add(nearestBeacon.ToString());
                var currentFitness = EvaluateFitness(start, nearestBeacon.Location, target);
                var radius = 100;
                while (radius < 2000)
                {
                    var beaconsAround = beacons.Where(item =>
                        ((item.Location.X > nearestBeacon.Location.X - radius
                       && item.Location.X < nearestBeacon.Location.X + radius)
                       && item.Location.Y > nearestBeacon.Location.Y - radius
                       && item.Location.Y < nearestBeacon.Location.Y + radius)
                       && EvaluateFitness(start, item.Location, target) < currentFitness
                       && !visited.Contains(item.ToString())).ToList();

                    if (beaconsAround.Count > 0)
                    {
                        nearestBeacon = beaconsAround.OrderByDescending(item => EvaluateFitness(start, item.Location, target)).LastOrDefault(); ;
						path.Add(nearestBeacon);
                        break;
                    }

                    radius += 100;
                }

                //  next
                limit++;
            }
            return path;
        }

        private SharedBeacon GetCurrnetNearestBeacon(Point target)
        {
			//var visibleBeacons = new List<SharedBeacon>();

			//foreach (var beacon in beaconsHandler.VisibleData.Take(3))
			//{
			//	visibleBeacons.Add(beacon);
			//}
			var visibleBeacons = beaconsHandler.VisibleData.Clone().Take(3);
			var _location = beaconsHandler.Position.Location;
			var result = visibleBeacons.OrderByDescending(x => EvaluateFitness(_location, x.Location, target)).LastOrDefault();
			return result;
        }

        private double EvaluateFitness(Point start, Point current, Point target)
        {
            // TODO: vzdialenost k cielu ^2 - vzdialenost odomna
            var distanceToTarget = current.Distance(target);


            return distanceToTarget;
        }
    }
}
