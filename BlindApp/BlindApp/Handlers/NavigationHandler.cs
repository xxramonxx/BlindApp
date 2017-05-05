﻿using BlindApp.Database.Tables;
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
    public  class NavigationHandler
    {
        public static NavigationHandler Instance
        {
            get
            {
                if ( _instance == null){
                    _instance = new NavigationHandler();
                }
                return _instance;
            }
        }
        private static NavigationHandler _instance;
       
        public Position Position { get; set; }
        public Queue<MapPoint> Path { get; set; }

        public  MapPoint NextMilestone
        {
            get { return Path.Count > 0 ? Path.Peek() : new MapPoint(); }
        }
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
            Position = new Position();

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

        public void Find (Target target)
        {
            RemainingMeters = 0;
            DestinationReached = false;

            if(App.DEBUG)
            {

                Position.XCoordinate = 4800;
                Position.YCoordinate = -1000;
            }

            var path = NewFind(Position.Location, target.Location);

            // HACK target ako beacon
            path.Enqueue(new MapPoint
            {
                XCoordinate = target.Location.X,
                YCoordinate = target.Location.Y
            });

            if (path.Count == 0) return;

            Path = path.Clone();
            RemainingMeters = GetPathDistance();

            StartNavigation();
        }

        private void StartNavigation()
        {
            DependencyService.Get<ICustomThread>().Thread += delegate
            {
               Device.StartTimer(TimeSpan.FromSeconds(1), delegate
               {
                   
                   return true;
               });
           };
        }

        private  double GetPathDistance()
        {
            double distance = 0;

            Point current;
            var next = Path.Dequeue().Location;
            distance += Position.Location.Distance(next);

            current = next;
            while(Path.Count > 0)
            {
                distance += Path.Peek().Location.Distance(current);
                current = Path.Dequeue().Location;
            }

            return distance / 100;
        }

        public static Queue<MapPoint> NewFind(Point start, Point target)
        {
            var path = new Queue<MapPoint>();

            var beacons = Building.Beacons;

            SharedBeacon nearestBeacon = GetCurrnetNearestBeacon(target);
            path.Enqueue(nearestBeacon);

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
                        path.Enqueue(nearestBeacon);
                        break;
                    }

                    radius += 100;
                }

                //  next
                limit++;
            }
            return path;
        }

        private static SharedBeacon GetCurrnetNearestBeacon(Point target)
        {
            var visibleBeacons = App.BeaconsHandler.VisibleData.Clone<List<SharedBeacon>>().Take(4);
            var position = NavigationHandler.Instance.Position.Location;

            return visibleBeacons.OrderByDescending(x => EvaluateFitness(position, x.Location, target)).LastOrDefault();
        }

        private static double EvaluateFitness(Point start, Point current, Point target)
        {
            // TODO: vzdialenost k cielu ^2 - vzdialenost odomna
            var distanceToTarget = current.Distance(target);


            return distanceToTarget;
        }
    }
}
