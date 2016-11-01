using Xamarin.Forms;

using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;

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
                    beaconList[beacon.UID + beaconList.Count] = beacon;
             //       beaconList[beacon.UID] = beacon;
                }

                var sortedList = beaconList.OrderBy(b => b.Value.Distance);
                beaconList = sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);

                Data = beaconList.Values.ToList();
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
            ListChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

