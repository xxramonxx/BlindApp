using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;
using BlindApp.Model;

namespace BlindApp.Database.Tables
{
    public class TargetsTable
    {
        SQLiteConnection sqlite;

        public TargetsTable(SQLiteConnection sqlite)
        {
            this.sqlite = sqlite;
        }

        public bool TableExistence() { return sqlite.GetTableInfo("Targets").Any(); }

        public void CreateTable() { sqlite.CreateTable<Target>(); }
        public void WipeTable() { sqlite.DeleteAll<Target>(); }
        public void DropTable() { sqlite.DropTable<Target>(); }

        public void Insert(Target project) { sqlite.Insert(project); }
        public void Update(Target project) { sqlite.Update(project); }
        public void Execute(string query) { sqlite.Execute(query); }
        public void Delete(Target project) { sqlite.Delete(project); }

        public Target SelectSingleRow(string query) { return sqlite.Query<Target>(query).FirstOrDefault(); }
        public List<Target> SelectMoreRows(string query) { return sqlite.Query<Target>(query); }

        public Target GetByID(int ID) { return sqlite.Get<Target>(ID); }

        public List<Target> GetTargetsByName(string param)
        {
            var result = SelectMoreRows("select * from Targets WHERE EmployeeParsed LIKE '%" + param + "%'");
            if (result.Count == 0)
            {
                var keywords = param.Split(' ');
                foreach (var key in keywords.Reverse())
                {
                    result = SelectMoreRows("select * from Targets WHERE EmployeeParsed LIKE '%" + key + "%'");
                    if (result.Count > 0)
                        return result;
                }
            }
            return result;
        }

        public int GetLastID() { return sqlite.ExecuteScalar<int>("SELECT last_insert_rowid()"); }
    }
}