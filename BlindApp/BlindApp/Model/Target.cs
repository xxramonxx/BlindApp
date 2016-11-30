using BlindApp.Database;
using BlindApp.Database.Tables;
using SQLite.Net.Attributes;
using System;

namespace BlindApp.Model
{
    [Table("Targets")]
    public class Target : IComparable<Target>
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        public string Employee { get; set; }
        [Ignore]
        public string EmployeeParsed { get; set; }
        public string Office { get; set; }
        public int Floor { get; set; }
        public int Room { get; set; }
        public int Minor { get; set; }

        public string GetNearestEndpoint()
        {
            PointsTable pointsTable = new PointsTable(Initialize.DatabaseConnect());

            var endpoint = pointsTable.SelectSingleRow(
                "select * from Points Where Floor=" + Floor +
                " AND Minor=" + Minor);
            return endpoint.ToString();
        }

        public int CompareTo(Target that)
        {
            if (string.Compare(this.Employee, that.Employee, StringComparison.CurrentCulture) < 0) return -1;
            if (this.Employee == that.Employee) return 0;
            return 1;
        }
    }
}
