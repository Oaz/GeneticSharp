using System;
using System.Linq;
using System.ComponentModel;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Extensions.Stretch;
using GeneticSharp.Infrastructure.Framework.Texts;
using Gtk;

namespace GeneticSharp.Runner.GtkApp.Samples
{
  [DisplayName("Stretch")]
  public class StretchSampleController : SampleControllerBase
  {
    public StretchSampleController()
    {
    }

    private string m_problemDefinition;
    private StretchFitness m_fitness;
    private bool m_showPairs;
    private Func<int, int, int> ScaleX;
    private Func<int, int, int> ScaleY;

    public override IChromosome CreateChromosome()
    {
      return new StretchChromosome(m_fitness.Width * m_fitness.Height, m_fitness.Nodes.Length);
    }

    public override Widget CreateConfigWidget()
    {
      var container = new VBox();

      var loadButton = new Button();
      loadButton.Label = "Load Nodes...";
      loadButton.Clicked += delegate
      {
        var dialog = new Gtk.FileChooserDialog(
          "Select node files", Context.GtkWindow, FileChooserAction.Open,
          "Cancel", ResponseType.Cancel,
          "Open", ResponseType.Accept
        );
        dialog.Filter = new FileFilter();
        dialog.Filter.AddPattern("*.txt");
        dialog.Filter.AddPattern("*.csv");
        if (dialog.Run() == (int)ResponseType.Accept)
        {
          m_problemDefinition = System.IO.File.ReadAllText(dialog.Filename);
          OnReconfigured();
        }
        dialog.Destroy();
      };
      container.Add(loadButton);

      m_showPairs = true;
      var showPairs = new CheckButton();
      showPairs.Active = m_showPairs;
      showPairs.Label = "Show pairs";
      showPairs.Toggled += delegate
      {
        m_showPairs = showPairs.Active;
      };
      container.Add(showPairs);

      return container;
    }

    public override ICrossover CreateCrossover()
    {
      return new OrderedCrossover();
    }

    public override IFitness CreateFitness()
    {
      if (string.IsNullOrEmpty(m_problemDefinition))
        return null;
      try
      {
				m_fitness = new StretchFitness(m_problemDefinition);
				InitializeDrawingScales();
				return m_fitness;
			}
      catch(Exception)
      {
				var md = new MessageDialog(Context.GtkWindow,
    			DialogFlags.DestroyWithParent, MessageType.Error,
    			ButtonsType.Close, "Error loading file");
  				md.Run();
  				md.Destroy();
        return null;
      }
    }

    private void InitializeDrawingScales()
    {
      var r = Context.DrawingArea;
      var margin = new { Horizontal = 10, Vertical = 10 };
      var box = new
      {
        X1 = r.Left + margin.Horizontal,
        Y1 = r.Top + margin.Vertical,
        X2 = r.Right - margin.Horizontal,
        Y2 = r.Bottom - margin.Vertical
      };
      var cell = new
      {
        Width = (r.Width - 2 * margin.Horizontal) / m_fitness.Width,
        Height = (r.Height - 2 * margin.Vertical) / m_fitness.Height
      };

      ScaleX = (x, s) => box.X1 + x * cell.Width + (s > 0 ? cell.Width / s : 0);
      ScaleY = (y, s) => box.Y1 + y * cell.Height + (s > 0 ? cell.Height / s : 0);
    }

    public override IMutation CreateMutation()
    {
      return new ReverseSequenceMutation();
    }

    public override ISelection CreateSelection()
    {
      return new EliteSelection();
    }

    public override void Draw()
    {
      if (m_fitness == null)
        return;
      DrawGrid();
      var bestChromosome = (StretchChromosome)Context.Population?.BestChromosome;
      if (bestChromosome == null)
        return;
      DrawPairs(bestChromosome);
      DrawNodes(bestChromosome);
    }

    private void DrawGrid()
    {
      Context.GC.RgbFgColor = new Gdk.Color(0, 0, 0);
      Context.GC.SetLineAttributes(1, Gdk.LineStyle.Solid, Gdk.CapStyle.Butt, Gdk.JoinStyle.Round);
      for (int x = 0; x <= m_fitness.Width; x++)
        Context.Buffer.DrawLine(Context.GC, ScaleX(x, 0), ScaleY(0, 0), ScaleX(x, 0), ScaleY(m_fitness.Height, 0));
      for (int y = 0; y <= m_fitness.Height; y++)
        Context.Buffer.DrawLine(Context.GC, ScaleX(0, 0), ScaleY(y, 0), ScaleX(m_fitness.Width, 0), ScaleY(y, 0));
    }

    private void DrawNodes(StretchChromosome chromosome)
    {
      foreach (var p in m_fitness.GetPositionsFor(chromosome))
      {
        Context.Layout.SetMarkup("<span color='blue'>{0}</span>".With(p.Node.Name));
        Context.Buffer.DrawLayout(Context.GC, ScaleX(p.X, 0), ScaleY(p.Y, 0), Context.Layout);
      }
    }

    private void DrawPairs(StretchChromosome chromosome)
    {
      if (!m_showPairs)
        return;
      Context.GC.RgbFgColor = new Gdk.Color(255, 0, 0);
      foreach (var pair in m_fitness.GetPairsFor(chromosome))
      {
        if (pair.Weigth == 0)
          continue;
        Context.GC.SetLineAttributes(pair.Weigth, Gdk.LineStyle.DoubleDash, Gdk.CapStyle.Butt, Gdk.JoinStyle.Round);
        Context.Buffer.DrawLine(Context.GC, ScaleX(pair.P1.X, 2), ScaleY(pair.P1.Y, 2), ScaleX(pair.P2.X, 2), ScaleY(pair.P2.Y, 2));
      }
    }

    public override void Reset()
    {
    }

    public override void Update()
    {
    }
  }
}
