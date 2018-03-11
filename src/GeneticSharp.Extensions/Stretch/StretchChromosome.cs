using System;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace GeneticSharp.Extensions.Stretch
{
  /// <summary>
  /// Stretch chromosome. Each genes represent the position of a node on the grid.
  /// </summary>
  public class StretchChromosome : ChromosomeBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:GeneticSharp.Extensions.Stretch.StretchChromosome"/> class.
    /// </summary>
    public StretchChromosome(int numberOfCells, int numberOfNodes, params int[] indexes) : base(numberOfNodes)
    {
      m_numberOfCells = numberOfCells;
      m_numberOfNodes = numberOfNodes;
      var nodesIndexes = indexes.Length == 0 ? RandomizationProvider.Current.GetUniqueInts(numberOfNodes, 0, numberOfCells) : indexes;
			for (int i = 0; i < numberOfNodes; i++)
			{
				ReplaceGene(i, new Gene(nodesIndexes[i]));
			}
		}

		/// <summary>
		/// Creates a new chromosome using the same structure of this.
		/// </summary>
		/// <returns>The new.</returns>
		public override IChromosome CreateNew()
    {
      return new StretchChromosome(m_numberOfCells, m_numberOfNodes);
    }

    /// <summary>
    /// Generates the gene.
    /// </summary>
    /// <returns>The gene.</returns>
    /// <param name="geneIndex">Gene index.</param>
    public override Gene GenerateGene(int geneIndex)
    {
			return new Gene(RandomizationProvider.Current.GetInt(0, m_numberOfCells));
		}

    private int m_numberOfCells;
    private int m_numberOfNodes;
  }
}
