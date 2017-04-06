
using BlindApp.Database;
using BlindApp.Database.Tables;
using SQLite.Net.Attributes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BlindApp
{
    [Table("Points")]
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
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public double ZCoordinate { get; set; }
        public Point Location {
            get
            {
                return new Point(XCoordinate, YCoordinate);
            }
        }
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
                XCoordinate = additionalData.XCoordinate;
                YCoordinate = additionalData.YCoordinate;
                ZCoordinate = additionalData.ZCoordinate;
                Floor = additionalData.Floor;
            }
        }
    }
}
