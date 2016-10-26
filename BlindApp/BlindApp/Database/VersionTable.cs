using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace BlindApp.Database.Tables
{
	[Table("DatabaseVersion")]
	public class DatabaseVersion
	{
		[PrimaryKey, AutoIncrement, Column("ID")]
		public int ID { get; set; }

		public int Version { get; set; }
	}

	public class DatabaseVersionTable
	{
		SQLiteConnection sqlite;

		public DatabaseVersionTable(SQLiteConnection sqlite)
		{
			this.sqlite = sqlite;
		}

		public Boolean TableExistence() { return sqlite.GetTableInfo("DatabaseVersion").Any(); }

		public void CreateTable() { sqlite.CreateTable<DatabaseVersion>(); }
		public void DeleteTable() { sqlite.DeleteAll<DatabaseVersion>(); }

		public void Insert(DatabaseVersion project) { sqlite.Insert(project); }
		public void Update(DatabaseVersion project) { sqlite.Update(project); }
		public void Query(String query) { sqlite.Query<DatabaseVersion>(query); }
		public void Delete(DatabaseVersion project) { sqlite.Delete(project); }

		public int ActualVersion() { return sqlite.ExecuteScalar<int>("SELECT Version FROM DatabaseVersion ORDER BY ID DESC"); }
	}
}