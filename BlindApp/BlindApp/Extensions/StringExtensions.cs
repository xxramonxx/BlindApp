using System;
using Xamarin.Forms;

namespace BlindApp
{
	public static class StringExtensions
	{
		public static string TrimAndReduce(this string str)
		{
			return DependencyService.Get<IStringExtensions>().TrimAndReduce(str);
		}

		public static string ConvertWhitespacesToSingleSpaces(this string value)
		{
			return DependencyService.Get<IStringExtensions>().ConvertWhitespacesToSingleSpaces(value);
		}

		public static string RemoveDiacritics(this string text)
		{
			return DependencyService.Get<IStringExtensions>().RemoveDiacritics(text);
		}

		public static string CapitalizeFirstLetters(this string text)
		{
			return DependencyService.Get<IStringExtensions>().CapitalizeFirstLetters(text);
		}
	}
}
