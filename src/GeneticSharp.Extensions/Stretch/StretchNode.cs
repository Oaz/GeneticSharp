using System;
using System.Linq;

namespace GeneticSharp.Extensions.Stretch
{
  /// <summary>
  /// A Node.
  /// </summary>
  public class StretchNode
  {
    /// <summary>
    /// Creates a node from a serialized definition
    /// </summary>
    public StretchNode(string definition)
    {
      var split = StretchParser.Tokens(definition);
      Name = split[0];
      Weights = split.Skip(1).Select(x => int.Parse(x)).ToArray();
    }

		/// <summary>
		/// Name of the node
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Weights of the couples with each other node
		/// </summary>
		public int[] Weights { get; private set; }
	}

  /// <summary>
  /// Coordinates of a node within the grid.
  /// </summary>
  public struct StretchPosition
  {
    /// <summary>
    /// The node.
    /// </summary>
    public StretchNode Node;

    /// <summary>
    /// X
    /// </summary>
    public int X;

    /// <summary>
    /// Y
    /// </summary>
    public int Y;
  }

  /// <summary>
  /// Pair of nodes and associated weight
  /// </summary>
  public struct StretchPair
  {
    /// <summary>
    /// First node
    /// </summary>
		public StretchPosition P1;

    /// <summary>
    /// Second node
    /// </summary>
		public StretchPosition P2;

    /// <summary>
    /// The weigth.
    /// </summary>
    public int Weigth;
	}
}
