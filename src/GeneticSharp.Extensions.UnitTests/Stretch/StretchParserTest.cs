using System;
using GeneticSharp.Extensions.Stretch;
using NUnit.Framework;

namespace GeneticSharp.Extensions.UnitTests.Stretch
{
	[TestFixture]
	[Category("Extensions")]
	public class StretchParserTest
	{
		[Test]
		public void ParseText()
		{
			Assert.That(StretchParser.Rows("a\n8\nb"), Is.EqualTo(new string[] { "a", "8", "b" }));
		}

		[Test]
		public void IgnoreEmptyRows()
		{
			Assert.That(StretchParser.Rows("a\n\n8\nb\n"), Is.EqualTo(new string[] { "a", "8", "b" }));
		}

		[Test]
		public void ParseRow()
		{
			Assert.That(StretchParser.Tokens("a 8 b 3 c"), Is.EqualTo(new string[] { "a", "8", "b", "3", "c" }));
		}

		[Test]
		public void ParseRowWithMultipleSeparators()
		{
			Assert.That(
        StretchParser.Tokens("a\t2;3,0    b \tc,;8\t\td"),
        Is.EqualTo(new string[] { "a", "2", "3", "0", "b", "c", "8", "d" })
      );
		}
	}
}
