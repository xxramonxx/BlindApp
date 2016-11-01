
using BlindApp.Database;
using BlindApp.Database.Tables;
using System;
using System.Diagnostics;

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
        public string XCoordinate { get; set; }
        public string YCoordinate { get; set; }
        public string ZCoordinate { get; set; }
        public string Floor { get; set; }
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

        public void UpdateData(SharedBeacon Beacon)
        {
            Distance = Beacon.Distance;
            Rssi = Beacon.Rssi;
            LastUpdate = (Stopwatch.GetTimestamp() / Stopwatch.Frequency);
        }

        public void LoadAdditionalData()
        {
            UID = UID.ToUpper();   // lazy hack

            PointsTable pointsTable = new PointsTable(Initialize.DatabaseConnect());
            
            var additionalData = pointsTable.SelectSingleRow(
                "select * from Points Where UID='" + UID +
                "' AND Minor=" + Minor + " AND Major=" + this.Major
                );

            ID = additionalData.ID;
            XCoordinate = additionalData.XCoordinate;
            YCoordinate = additionalData.YCoordinate;
            ZCoordinate = additionalData.ZCoordinate;
            Floor = additionalData.Floor;
        }
    }
}
