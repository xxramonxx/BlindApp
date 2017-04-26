using Xamarin.Forms;

using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using BlindApp.Model;
using System.Threading.Tasks;

namespace BlindApp
{
    public class BeaconsHandler
    {
        private readonly int DELETE_INTERVAL_SECONDS = 50;
        private Dictionary<string, SharedBeacon> beaconList;

        public event EventHandler ListChanged;   
        public List<SharedBeacon> VisibleData { get; set; }

        public BeaconsHandler()
        {
            VisibleData = new List<SharedBeacon>();
            beaconList = new Dictionary<string, SharedBeacon>();
        }

        public void Init()
        {
     /*       var beaconService = DependencyService.Get<IAltBeaconService>();

            beaconService.ListChanged += (sender, e) =>
            {
                foreach ( var beacon in e.Data)
                {
                    var pattern = new StringBuilder(beacon.Major + beacon.Minor).ToString();
                 //   pattern = new StringBuilder(beacon.UID + beaconList.Count).ToString(); // test purposes only
                    if (beaconList.ContainsKey(pattern))
                    {
                        beaconList[pattern].UpdateData(beacon);
                    } else {
                        beacon.LoadAdditionalData();
                        beaconList[pattern] = beacon;
                    }                  
                }*/

                #region DEBUG

                SharedBeacon[] beacons = {
                    new SharedBeacon
                    {
                        UID="beacon38",
                        Major = "3",
                        Minor = "38",
                        Distance = 1,
                        XCoordinate=4572.08,
                        YCoordinate=-717.06
                    },
                    new SharedBeacon
                    {
                        UID="beacon39",
                        Major = "3",
                        Minor = "39",
                        Distance = 1,
                        XCoordinate=5570.40,
                        YCoordinate=-722.00
                    },
                    new SharedBeacon
                    {
                        UID="beacon53",
                        Major = "3",
                        Minor = "53",
                        Distance = 4,
                        YCoordinate=-1611.61,
                        XCoordinate=5075.84
                    },
                };

                foreach (var beacon in beacons)
                {
                    var pattern = new StringBuilder(beacon.UID + beacon.Major + beacon.Minor).ToString();
                   
                     beaconList[pattern] = beacon;
                }

                #endregion

                var sortedList = beaconList.OrderBy(b => b.Value.Distance);
                beaconList = sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);

                OnListChanged();
            /*   };*/

            InitAgingAlgorithm();
            InitLocationService();

     //       beaconService.InitializeService();
        }

        private void InitAgingAlgorithm()
        {
            Task.Run(() =>
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    // Beacons aging algorithm
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
                    // Returning true means you want to repeat this timer
                    return true;
                });
            });
        }

        private void InitLocationService()
        {
            Task.Run(() =>
            {
                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    Debug.WriteLine(beaconList.Count);
                    if (beaconList.Count >= 3)
                    {
                        NavigationHandler.Position.Localize(beaconList.Values.ToList());
                    }
                    // Returning true means you want to repeat this timer
                    return true;
                });
            });
        }

      

        private void OnListChanged()
        {
            VisibleData = beaconList.Values.ToList();
            ListChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

