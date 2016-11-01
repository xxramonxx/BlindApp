
using System;
using System.Diagnostics;

namespace BlindApp
{
    public class SharedBeacon
    {
        public int ID { get; set; }
        public double Distance { get; set; }
        public int Rssi { get; set; }
        public string FormatedDistance {
            get { return string.Format("{0:N2}m", Distance); }
        }
        public string UID { get; set; }
        public string MAC { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public long LastUpdate { get; set; }
        // Extra data //
        public Array Properties { get; set; }

        public SharedBeacon()
        {
            this.LastUpdate = (Stopwatch.GetTimestamp() / Stopwatch.Frequency);
        }
    }
}
