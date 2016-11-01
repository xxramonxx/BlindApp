using Xamarin.Forms;

using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace BlindApp
{
    public class BeaconViewModel
    {
        private readonly int DELETE_INTERVAL_SECONDS = 5;
        private Dictionary<string, SharedBeacon> beaconList;

        public event EventHandler ListChanged;   
        public List<SharedBeacon> Data { get; set; }

        public BeaconViewModel()
        {
            Data = new List<SharedBeacon>();
            beaconList = new Dictionary<string, SharedBeacon>();
        }

        public void Init()
        {
            var beaconService = DependencyService.Get<IAltBeaconService>();

            beaconService.ListChanged += (sender, e) =>
            {
                foreach ( var beacon in e.Data)
                {
                    var pattern = new StringBuilder(beacon.UID + beacon.Major + beacon.Minor).ToString();
                 //   pattern = new StringBuilder(beacon.UID + beaconList.Count).ToString(); // test purposes only
                    if (beaconList.ContainsKey(pattern))
                    {
                        beaconList[pattern].UpdateData(beacon);
                    } else {
                        beacon.LoadAdditionalData();
                        beaconList[pattern] = beacon;
                    }
                    
                }

                var sortedList = beaconList.OrderBy(b => b.Value.Distance);
                beaconList = sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);

                OnListChanged();
            };

            Device.StartTimer( TimeSpan.FromSeconds(1), () => {
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

            beaconService.InitializeService();
        }

        private void OnListChanged()
        {
            Data = beaconList.Values.ToList();
            ListChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

