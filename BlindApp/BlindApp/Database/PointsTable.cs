using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;
using System.Text;

namespace BlindApp.Database.Tables
{
	public class PointsTable
    {
		SQLiteConnection sqlite;

		public PointsTable(SQLiteConnection sqlite)
		{
			this.sqlite = sqlite;
		}

		public Boolean TableExistence() { return sqlite.GetTableInfo("Points").Any(); }

		public void CreateTable() { sqlite.CreateTable<SharedBeacon>(); }
		public void WipeTable() { sqlite.DeleteAll<SharedBeacon>(); }
        public void DropTable() { sqlite.DropTable<SharedBeacon>(); }

        public void Insert(SharedBeacon project) { sqlite.Insert(project); }
		public void Update(SharedBeacon project) { sqlite.Update(project); }
		public void Execute(string query) { sqlite.Execute(query); } 
		public void Delete(SharedBeacon project) { sqlite.Delete(project); }

		public SharedBeacon SelectSingleRow(String query) { return sqlite.Query<SharedBeacon>(query).FirstOrDefault();}
		public List<SharedBeacon> SelectMoreRows(String query) { return sqlite.Query<SharedBeacon>(query); }

		public SharedBeacon GetByID(int ID) { return sqlite.Get<SharedBeacon>(ID); }

        public int GetLastID() { return sqlite.ExecuteScalar<int>("SELECT last_insert_rowid()"); }
	}
}