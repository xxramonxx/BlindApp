using System;
using System.Globalization;
using System.Text.RegularExpressions;
using BlindApp.Droid;
using Java.Lang;
using Java.Text;

[assembly: Xamarin.Forms.Dependency(typeof(StringExtensions))]
namespace BlindApp.Droid
{
	public class StringExtensions : IStringExtensions
	{
		// This is the extension method.
		// The first parameter takes the "this" modifier
		// and specifies the type for which the method is defined.
		public string TrimAndReduce(string str)
		{
			return ConvertWhitespacesToSingleSpaces(str).Trim();
		}

		public string ConvertWhitespacesToSingleSpaces(string value)
		{
			return Regex.Replace(value, @"\s+", " ");
		}

		public string RemoveDiacritics(string text)
		{
			var normalizedString = Normalizer.Normalize(text, Normalizer.Form.Nfd);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}
			return Normalizer.Normalize(stringBuilder.ToString(), Normalizer.Form.Nfc);
		}

		public string CapitalizeFirstLetters(string text)
		{
			var words = text.Split();
			string[] result = new string[words.Length];
			for (var i = 0; i < words.Length; i++)
			{
				if (words[i].Length > 1)
					result.SetValue(char.ToUpper(words[i][0]) + words[i].Substring(1), i);
				else
					result.SetValue(words[i].ToUpper(), i);
			}
			return string.Join(" ", result);
		}
	}
}