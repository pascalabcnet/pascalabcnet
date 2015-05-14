using System;
using System.Text.RegularExpressions;

namespace com.calitha.commons
{
	/// <summary>
	/// StringUtil contains a selection of convenience methods for dealing with strings.
	/// </summary>
	public sealed class StringUtil
	{

		private StringUtil()
		{}

		/// <summary>
		/// Replaces the characters null, alert, backspace, form feed,
		/// newline, carriage return, horizontal tab and vertical tab
		/// with their escaped version. This allows you to show
		/// the escape characters.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ShowEscapeChars(string str)
		{
			Regex regex = new Regex("\0|\a|\b|\f|\n|\r|\t|\v");
			MatchEvaluator evaluator = 
				new MatchEvaluator((new StringUtil()).MatchEvent);
			return regex.Replace(str,evaluator);
		}

		private string MatchEvent(Match match)
		{
			string m = match.ToString();
			if (m == "\0")
				return "\\0";
			else if (m == "\a")
				return "\\a";
			else if (m == "\b")
				return "\\b";
			else if (m == "\f")
				return "\\f";
			else if (m == "\n")
				return "\\n";
			else if (m == "\r")
				return "\\r";
			else if (m == "\t")
				return "\\t";
			else if (m == "\v")
				return "\\v";
			else
				return m;
		}


	}
}
