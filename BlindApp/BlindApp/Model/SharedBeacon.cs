﻿
using SQLite.Net.Attributes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using BlindApp.Model;
using System.Linq;

namespace BlindApp
{
    public class SharedBeacon : MapPoint
    {
        // Received data from bluetooth service 
        public string UID { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public int Rssi { get; set; }
        public string MAC { get; set; }
        public bool Initilized { get; set; }
        public long LastUpdate { get; set; }
        public double Distance { get; set; }

        // Extra data //
        public string FormatedDistance
        {
            get { return string.Format("{0:N2}m", Distance); }
        }

        public SharedBeacon()
        {
            LastUpdate = (Stopwatch.GetTimestamp() / Stopwatch.Frequency);
            Initilized = false;
        }

        public override string ToString()
        {
            var SB = new StringBuilder();
            SB.Append(UID).Append("-").Append(Major).Append("-").Append(Minor);
            return SB.ToString();
        }

        public void UpdateData(SharedBeacon Beacon)
        {
            Distance = Beacon.Distance;
            Rssi = Beacon.Rssi;
            LastUpdate = (Stopwatch.GetTimestamp() / Stopwatch.Frequency);
        }

        public void LoadAdditionalData()
        {
            UID = UID.ToUpper();   // lazy hack

            //PointsTable pointsTable = new PointsTable(Initializer.DatabaseConnect());

            //var additionalData = pointsTable.SelectSingleRow(
            //"select * from Points Where UID='" + UID +
            //"' AND Minor=" + Minor + " AND Major=" + this.Major
            //);
            if (Building.Beacons != null)
            {
                var additionalData = Building.Beacons.Where(item =>
                    (item.Major == this.Major
                     && item.Minor == this.Minor)).FirstOrDefault();
	            if (additionalData != null)
	            {
	                XCoordinate = additionalData.XCoordinate;
	                YCoordinate = additionalData.YCoordinate;
	                ZCoordinate = additionalData.ZCoordinate;
	                Floor = additionalData.Floor;
                    Initilized = true;
	            }
            }
        }
    }
}
