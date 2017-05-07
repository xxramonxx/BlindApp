using System;
namespace BlindApp
{
	public interface IFiles
	{
		void SaveFile(string filename, string text);
		string LoadFile(string filename);
	}
}