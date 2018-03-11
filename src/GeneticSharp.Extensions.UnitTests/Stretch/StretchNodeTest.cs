using System;
using GeneticSharp.Extensions.Stretch;
using NUnit.Framework;

namespace GeneticSharp.Extensions.UnitTests.Stretch
{
  [TestFixture]
	[Category("Extensions")]
	public class StretchNodeTest
  {
		[Test]
		public void DefineNodeFromString()
		{
			var node = new StretchNode("myName 2 3 0 5 0 8 1");
			Assert.That(node.Name, Is.EqualTo("myName"));
			Assert.That(node.Weights, Is.EqualTo(new int[] { 2, 3, 0, 5, 0, 8, 1 }));
		}
	}
}
