using System;
using System.Linq;
using GeneticSharp.Extensions.Stretch;
using NUnit.Framework;

namespace GeneticSharp.Extensions.UnitTests.Stretch
{
  [TestFixture]
  [Category("Extensions")]
  public class StretchChromosomeTest
  {
		[Test]
		public void ChromosomeGenesArePositionsWithinTheDefinedGrid()
		{
			var chromosome = new StretchChromosome(5, 2);
			var positions = chromosome.GetGenes().Select(g => (int)g.Value).ToArray();
			Assert.That(positions.Length, Is.EqualTo(2));
			Assert.That(positions[0], Is.Not.EqualTo(positions[1]));
			Assert.That(positions[0], Is.InRange(0, 4));
			Assert.That(positions[1], Is.InRange(0, 4));
		}

		[Test]
		public void ChromosomeGenesCanBeForcedForTestPurpose()
		{
			var chromosome = new StretchChromosome(5, 2, 4, 1);
			var positions = chromosome.GetGenes().Select(g => (int)g.Value).ToArray();
			Assert.That(positions[0], Is.EqualTo(4));
			Assert.That(positions[1], Is.EqualTo(1));
		}

		[Test]
		public void RandomChromosomesEnsureThatWhenTryingEnoughWeCanHaveFullyDifferentGenes()
		{
			var chromosomeRef = new StretchChromosome(5, 2);
			var positionsRef = chromosomeRef.GetGenes().Select(g => (int)g.Value).ToArray();
			for (;;)
			{
				var chromosome = new StretchChromosome(5, 2);
				var positions = chromosome.GetGenes().Select(g => (int)g.Value).ToArray();
				if (positions[0] != positionsRef[0] && positions[1] != positionsRef[1])
					break;
			}
		}


    [Test]
    public void RandomChromosomesEnsureThatWhenTryingEnoughWeCanAllKindOfGenes()
		{
      var kindOfGenes = Enumerable.Repeat(0,2*5).ToArray();
			for (;;)
			{
				var chromosome = new StretchChromosome(5, 2);
				var positions = chromosome.GetGenes().Select(g => (int)g.Value).ToArray();
				kindOfGenes[positions[0]] = 1;
				kindOfGenes[5+positions[1]] = 1;
				if (kindOfGenes.Sum() == 10)
					break;
			}
		}

		[Test]
		public void NewChromosomeHasSameStructureAsExistingOne()
		{
			var chromosomeRef = new StretchChromosome(5, 2);
			var chromosome = chromosomeRef.CreateNew();
			var positions = chromosome.GetGenes().Select(g => (int)g.Value).ToArray();
			Assert.That(positions.Length, Is.EqualTo(2));
			Assert.That(positions[0], Is.Not.EqualTo(positions[1]));
			Assert.That(positions[0], Is.InRange(0, 4));
			Assert.That(positions[1], Is.InRange(0, 4));
		}

		[Test]
		public void RandomChromosomesEnsureThatWhenTryingEnoughNewChromosomeHasFullyDifferentGenes()
		{
			var chromosomeRef = new StretchChromosome(5, 2);
			var positionsRef = chromosomeRef.GetGenes().Select(g => (int)g.Value).ToArray();
			for (;;)
			{
				var chromosome = chromosomeRef.CreateNew();
				var positions = chromosome.GetGenes().Select(g => (int)g.Value).ToArray();
				if (positions[0] != positionsRef[0] && positions[1] != positionsRef[1])
					break;
			}
		}


		[Test]
		public void GeneForSpecifiedIndexCanHaveAllKindOfExpectedValues()
		{
			var kindOfGenes = Enumerable.Repeat(0, 5).ToArray();
			var chromosome = new StretchChromosome(5, 2);
			for (;;)
			{
        var newGene = chromosome.GenerateGene(0);
        kindOfGenes[(int)newGene.Value] = 1;
				if (kindOfGenes.Sum() == 5)
					break;
			}
		}
	}
}
