using System;
namespace GeneticSharp.Extensions.Stretch
{
  /// <summary>
  /// Utility to define Stratch problem data from strings.
  /// </summary>
  public static class StretchParser
  {
    /// <summary>
    /// Split a text into rows
    /// </summary>
		public static string[] Rows(string content)
		{
			return content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}

    /// <summary>
    /// Split a row into tokens
    /// </summary>
		public static string[] Tokens(string row)
		{
			return row.Split(new char[] { ' ', '\t', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
