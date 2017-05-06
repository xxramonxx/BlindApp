﻿using Xamarin.Forms;

using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using BlindApp.Model;
using System.Threading;
using System.Threading.Tasks;
using BlindApp.Interfaces;
using PropertyChanged;

namespace BlindApp
{
	[ImplementPropertyChanged]
    public class BeaconsHandler
    {
        readonly int DELETE_INTERVAL_SECONDS = 15;
        Dictionary<string, SharedBeacon> beaconList = new Dictionary<string, SharedBeacon>();

        public event EventHandler ListChanged;   
        public List<SharedBeacon> VisibleData = new List<SharedBeacon>();

		public Position Position { get; set; } = new Position();

		public void Init()
        {
            if (App.DEBUG)
            {
                SharedBeacon[] beacons = {
                    new SharedBeacon
                    {
                        UID="beacon38",
                        Major = "1",
                        Minor = "119",
                        Distance = 1,
                        XCoordinate=1279.00,
                        YCoordinate=-2207.00
                    },
                    new SharedBeacon
                    {
                        UID="beacon39",
                        Major = "1",
                        Minor = "34",
                        Distance = 3,
                        XCoordinate=1186.00,
                        YCoordinate=-1836.00
                    },
                    new SharedBeacon
                    {
                        UID="beacon53",
                        Major = "1",
                        Minor = "124",
                        Distance = 3,
						XCoordinate=1038.00,
                        YCoordinate=-2570.00
                    },
                };
                foreach (var beacon in beacons)
                {
                    var pattern = new StringBuilder(beacon.UID + beacon.Major + beacon.Minor).ToString();

                    beaconList[pattern] = beacon;
                }
                var sortedList = beaconList.OrderBy(b => b.Value.Distance);
                beaconList = sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);

                OnListChanged();
            }
            else
            {
               var beaconService = DependencyService.Get<IAltBeaconService>();

                beaconService.ListChanged += (sender, e) =>
                {
                    foreach (var beacon in e.Data)
                    {
                        var pattern = new StringBuilder(beacon.Major + beacon.Minor).ToString();
                         //   pattern = new StringBuilder(beacon.UID + beaconList.Count).ToString(); // test purposes only
                         if (beaconList.ContainsKey(pattern))
                        {
                            beaconList[pattern].UpdateData(beacon);
                        }
                        else
                        {
                            beacon.LoadAdditionalData();

                            if (beacon.Initilized == true)
                                beaconList[pattern] = beacon;
                        }
                    }
	                var sortedList = beaconList.OrderBy(b => b.Value.Distance);
	                beaconList = sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);

	                OnListChanged();
               };

                InitAgingAlgorithm();
                InitLocationService();

                beaconService.InitializeService();
            }           
        }

        private void InitAgingAlgorithm()
        {
			var thread = DependencyService.Get<IThreadManager>();
			thread.ThreadDelegate += delegate
            {
                 Device.StartTimer(TimeSpan.FromSeconds(1), delegate
                 {
                     var time = (Stopwatch.GetTimestamp() / Stopwatch.Frequency);
                     foreach (var key in beaconList.Keys.ToList())
                     {
                         if (time - beaconList[key].LastUpdate > DELETE_INTERVAL_SECONDS)
                         {
                             beaconList.Remove(key);
                             OnListChanged();

                             Debug.WriteLine("Deleting beacon with UID: " + key);
                         }
                     }
                     return true;
                 });
            };
			thread.Start(thread.CreateNewThread());
        }

        private void InitLocationService()
        {
			var thread = DependencyService.Get<IThreadManager>();
			thread.ThreadDelegate += delegate
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), delegate
                {
                    if (beaconList.Count >= 3)
                    {
						Position.NewLocalize(beaconList.Values.ToList());
                    }
                    return true;
                });
            };
			thread.Start(thread.CreateNewThread());
        }
      
        private void OnListChanged()
        {
            VisibleData = beaconList.Values.ToList();
            ListChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

