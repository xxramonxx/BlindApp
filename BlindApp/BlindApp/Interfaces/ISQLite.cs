using System;
using SQLite.Net;

namespace Prototyper.Interface
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection(String DatabaseFile);
	}
}