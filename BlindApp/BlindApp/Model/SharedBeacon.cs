
using BlindApp.Database;
using BlindApp.Database.Tables;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace BlindApp
{
    public class SharedBeacon
    {
        // Received data from bluetooth service 
        public string UID { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public double Distance { get; set; }
        public int Rssi { get; set; }
        public string MAC { get; set; }
        public long LastUpdate { get; set; }
        //Data from database
        public int ID { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public double ZCoordinate { get; set; }
        public double Floor { get; set; }
//        public Array Properties { get; set; }
        // Extra data //
        public string FormatedDistance
        {
            get { return string.Format("{0:N2}m", Distance); }
        }

        public SharedBeacon()
        {
            LastUpdate = (Stopwatch.GetTimestamp() / Stopwatch.Frequency);
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

            PointsTable pointsTable = new PointsTable(Initializer.DatabaseConnect());
            
            var additionalData = pointsTable.SelectSingleRow(
                "select * from Points Where UID='" + UID +
                "' AND Minor=" + Minor + " AND Major=" + this.Major
                );

            if (additionalData != null)
            {
                ID = additionalData.ID;
                XCoordinate = Double.Parse(additionalData.XCoordinate, CultureInfo.InvariantCulture);
                YCoordinate = Double.Parse(additionalData.YCoordinate, CultureInfo.InvariantCulture);
                ZCoordinate = Double.Parse(additionalData.ZCoordinate, CultureInfo.InvariantCulture);
                Floor = Double.Parse(additionalData.Floor);
            }
        }
    }
}
