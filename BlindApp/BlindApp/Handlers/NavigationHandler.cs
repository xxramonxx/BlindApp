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
        public  Queue<MapPoint> Path { get; set; }

        public  MapPoint NextMilestone
        {
            get { return Path.Count > 0 ? Path.Peek() : new MapPoint(); }
        }
        public  double RemainingMetersNextMilestone;
        public  long RemainingStepsNextMilestone
        {
            get { return Convert.ToInt32(RemainingMetersNextMilestone / STEP_METERS); }
        }

        public  long RemainingSteps
        {
            get { return Convert.ToInt32(RemainingMeters / STEP_METERS); }
        }
        public  double RemainingMeters;

        public  bool DestinationReached;

        private const double STEP_METERS = 1.5f;


        public NavigationHandler()
        {
            RemainingMeters = 0;
            DestinationReached = false;
            Position = new Position();
        }

        public  async void Find (Target target)
        {
            RemainingMeters = 0;
            DestinationReached = false;

            if(App.DEBUG)
            {

                Position.XCoordinate = 4800;
                Position.YCoordinate = -1000;
            }

            var path = Map.NewFind(Position.Location, target.Location);

            // HACK target ako beacon
            path.Enqueue(new MapPoint
            {
                XCoordinate = target.Location.X,
                YCoordinate = target.Location.Y
            });

            if (path.Count == 0) return;

            Path = path.Clone();
            RemainingMeters = GetPathDistance(path);

          //  StartNavigation();
        //    Debug.WriteLine(Path.Peek().Location.ToString());
        }

        private async Task<bool> StartNavigation()
        {
        //    await Task.Run( () => Do() );
    //        Task.Run( () =>
    //        {
				//Device.StartTimer(TimeSpan.FromSeconds(0.5), Do());
            //}).ConfigureAwait(false);

            return true;
        }

        private Func<bool> Do()
        {
            return () => true;
        }

        private  double GetPathDistance(Queue<MapPoint> path)
        {
            double distance = 0;

            Point current;
            var next = path.Dequeue().Location;
            distance += Position.Location.Distance(next);

            current = next;
            while(path.Count > 0)
            {
                distance += path.Peek().Location.Distance(current);
                current = path.Dequeue().Location;
            }

            return distance / 100;
        }
    }
}
