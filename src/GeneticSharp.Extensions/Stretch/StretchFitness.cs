using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace GeneticSharp.Extensions.Stretch
{
  /// <summary>
  /// A problem where we want to position a number p of nodes on a (n,m) grid
  /// so that distance between nodes is as big as possible.
  /// Being given a pair of nodes, their distance is weighted by a factor.
  /// In other words, we want to maximize the sum of all weighted distances between node pairs.
  /// </summary>
  public class StretchFitness : IFitness
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:GeneticSharp.Extensions.Stretch.StretchFitness"/> class.
    /// </summary>
    /// <param name="definition">Definition.</param>
    public StretchFitness(string definition)
    {
      var rows = StretchParser.Rows(definition);
      var sizes = StretchParser.Tokens(rows[0]);
      Width = int.Parse(sizes[0]);
      Height = int.Parse(sizes[1]);
      Nodes = rows.Skip(2).Select(r => new StretchNode(r)).ToArray();
      var sumOfAllWeights = Nodes.SelectMany(n => n.Weights).Sum() / 2;
      var gridDiagonal = Math.Sqrt(Math.Pow(Width - 1, 2) + Math.Pow(Height - 1, 2));
      m_totalWeightedDistanceUpperBound = sumOfAllWeights * gridDiagonal;
    }

    /// <summary>
    /// Compute the weighted the distance for a given chromosome.
    /// </summary>
    public IEnumerable<StretchPair> GetPairsFor(StretchChromosome chromosome)
    {
      var positions = GetPositionsFor(chromosome).ToArray();
      for (var node1 = 0; node1 < Nodes.Length - 1; node1++)
      {
        var pos1 = positions[node1];
        var p1 = new StretchPosition { Node = Nodes[node1], X = pos1.X, Y = pos1.Y };
        for (var node2 = node1 + 1; node2 < Nodes.Length; node2++)
        {
          var pos2 = positions[node2];
          var p2 = new StretchPosition { Node = Nodes[node2], X = pos2.X, Y = pos2.Y };
          var weight = Nodes[node1].Weights[node2];
          yield return new StretchPair { P1 = p1, P2 = p2, Weigth = weight };
        }
      }
    }

    /// <summary>
    /// Gets the positions for a given chromosome
    /// </summary>
    public IEnumerable<StretchPosition> GetPositionsFor(StretchChromosome chromosome)
    {
      return chromosome
        .GetGenes()
        .Select(g => (int)g.Value)
        .Select((index,i) => new StretchPosition { Node=Nodes[i], X = index % Width, Y = index / Width });
    }

    /// <summary>
    /// Compute the weighted the distance for a given chromosome.
    /// </summary>
    public double WeightedDistanceFor(StretchChromosome chromosome)
		{
			return GetPairsFor(chromosome)
        .Select(p => p.Weigth * Distance(p.P1,p.P2))
        .Sum();
		}

    private static double Distance(StretchPosition p1,StretchPosition p2)
    {
      return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }


		/// <summary>
		/// Width of the grid
		/// </summary>
		public int Width { get; private set; }

    /// <summary>
    /// Height of the grid
    /// </summary>
    public int Height { get; private set; }

		/// <summary>
		/// Nodes to put on the grid
		/// </summary>
		public StretchNode[] Nodes { get; private set; }

		/// <summary>
		/// Evaluate the specified chromosome.
		/// </summary>
		public double Evaluate(IChromosome chromosome)
    {
      return WeightedDistanceFor((StretchChromosome)chromosome)/ m_totalWeightedDistanceUpperBound;
    }

    private double m_totalWeightedDistanceUpperBound;
  }
}
