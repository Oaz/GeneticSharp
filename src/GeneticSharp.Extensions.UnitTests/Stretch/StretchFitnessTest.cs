using System;
using System.Linq;
using GeneticSharp.Extensions.Stretch;
using NUnit.Framework;

namespace GeneticSharp.Extensions.UnitTests.Stretch
{
  [TestFixture]
	[Category("Extensions")]
	public class StretchFitnessTest
  {
		[Test]
		public void DefineFitnessFromString()
		{
			var fitness = new StretchFitness(definitionSample);
			Assert.That(fitness.Width, Is.EqualTo(6));
			Assert.That(fitness.Height, Is.EqualTo(4));
			Assert.That(fitness.Nodes.Length, Is.EqualTo(5));
			Assert.That(
			  fitness.Nodes.Select(n => n.Name),
			  Is.EqualTo(new string[] { "Superman", "Kryptonite", "LexLuthor", "Batman", "Joker" })
			);
		}

    [Test]
    public void EvaluateFitnessAllInOneRow()
    {
      var fitness = new StretchFitness(definitionSample);
      var chromosome = new StretchChromosome(24, 5, 0, 1, 2, 3, 4);
      Assert.That(
        fitness.WeightedDistanceFor(chromosome),
        Is.EqualTo(1*7+2*4+3*2+1*6)
      );
      Assert.AreEqual(0.2437085, fitness.Evaluate(chromosome), precision);
    }

		[Test]
		public void EvaluateFitnessWithDiagonal()
		{
			var fitness = new StretchFitness(definitionSample);
			var chromosome = new StretchChromosome(24, 5, 0, 7, 14, 21, 16);
			var sqrt2 = Math.Sqrt(2);
			Assert.AreEqual(
			  fitness.WeightedDistanceFor(chromosome),
			  sqrt2 * 7 + 2 * sqrt2 * 4 + 3 * sqrt2 * 2 + sqrt2 * 6,
			  precision
			);
			Assert.AreEqual(0.3446559, fitness.Evaluate(chromosome), precision);
		}

		[Test]
		public void EvaluateGeneralFitness()
		{
			var fitness = new StretchFitness(definitionSample);
			var chromosome = new StretchChromosome(24, 5, 5, 12, 19, 0, 23);
			var diag = Math.Sqrt(34);
			var diag2 = Math.Sqrt(25);
			var diag3 = Math.Sqrt(29);
			Assert.AreEqual(
			  fitness.WeightedDistanceFor(chromosome),
			  diag3 * 7 + diag2 * 4 + 5 * 2 + diag * 6,
			  precision
			);
			Assert.AreEqual(0.9268313, fitness.Evaluate(chromosome), precision);
		}

		const string definitionSample = @"6 4
                 Superman Kryptonite LexLuthor Batman Joker
    Superman        0         7          4        2     0
    Kryptonite      7         0          0        0     0
    LexLuthor       4         0          0        0     0
    Batman          2         0          0        0     6
    Joker           0         0          0        6     0";

    const double precision = 0.0000001;
  }
}
