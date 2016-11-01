using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;

namespace BlindApp.Database.Tables
{
	[Table("Points")]
	public class Points
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
		public int ID { get; set; }
        public string UID { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string XCoordinate { get; set; }
        public string YCoordinate { get; set; }
        public string ZCoordinate { get; set; }
        public string Floor { get; set; }
        //      public List<String> Properties { get; set; }
    }

	public class PointsTable
    {
		SQLiteConnection sqlite;

		public PointsTable(SQLiteConnection sqlite)
		{
			this.sqlite = sqlite;
		}

		public Boolean TableExistence() { return sqlite.GetTableInfo("Points").Any(); }

		public void CreateTable() { sqlite.CreateTable<Points>(); }
		public void WipeTable() { sqlite.DeleteAll<Points>(); }
        public void DropTable() { sqlite.DropTable<Points>(); }

        public void Insert(Points project) { sqlite.Insert(project); }
		public void Update(Points project) { sqlite.Update(project); }
		public void Execute(String query) { sqlite.Execute(query); } 
		public void Delete(Points project) { sqlite.Delete(project); }

		public Points SelectSingleRow(String query) { return sqlite.Query<Points>(query).FirstOrDefault();}
		public List<Points> SelectMoreRows(String query) { return sqlite.Query<Points>(query); }

		public Points GetByID(int ID) { return sqlite.Get<Points>(ID); }

        public int GetLastID() { return sqlite.ExecuteScalar<int>("SELECT last_insert_rowid()"); }
	}
}