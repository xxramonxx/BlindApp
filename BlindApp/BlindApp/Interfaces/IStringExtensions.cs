using System;
namespace BlindApp
{
	public interface IStringExtensions
	{
		string TrimAndReduce(string str);
		string ConvertWhitespacesToSingleSpaces(string value);
		string RemoveDiacritics(string text);
		string CapitalizeFirstLetters(string text);
	}
}