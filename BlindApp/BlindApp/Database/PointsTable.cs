using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace BlindApp.Database.Tables
{
	[Table("Points")]
	public class Points
    {
		[PrimaryKey, AutoIncrement, Column("ID")]
		public int ID { get; set; }
        public string Distance { get; set; }
        public String UID { get; set; }
        public String MAC { get; set; }
        public String Minor { get; set; }
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
		public void DeleteTable() { sqlite.DeleteAll<Points>(); }

		public void Insert(Points project) { sqlite.Insert(project); }
		public void Update(Points project) { sqlite.Update(project); }
		public void Query(String query) { sqlite.Query<Points>(query); } 
		public void Delete(Points project) { sqlite.Delete(project); }

		public Points SelectSingleRow(String query) { return sqlite.ExecuteScalar<Points>(query); }
		public List<Points> SelectMoreRows(String query) { return sqlite.Query<Points>(query); }

		public Points GetByID(int ID) { return sqlite.Get<Points>(ID); }
		public int GetLastID() { return sqlite.ExecuteScalar<int>("SELECT last_insert_rowid()"); }
	}
}