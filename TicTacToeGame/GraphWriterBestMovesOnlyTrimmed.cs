using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeGame
{
    public class GraphWriterBestMovesOnlyTrimmed : GraphWriterBase
    {
        public GraphWriterBestMovesOnlyTrimmed(GraphOptions options) : base(options) { }

        public override void WriteDotFileHeaders(StreamWriter sw)
        {
            string fileHeader = $"strict digraph TicTacToe\n{{\ngraph[label=\"{Options.GraphLabel}\" fontname=\"{Options.FontName}\" fontcolor=\"{Options.NodeFontColor}\" size=\"48,36\" ratio=\"-1.33\" pad=\"1.5\" labelloc=t labeljust=c fontsize={Options.GraphLabelFontSize} overlap = \"false\" splines = \"true\" rankdir=TB ranksep=\"{Options.RankSeparation} equally\" colorscheme=\"{Options.GraphColorScheme}\" bgcolor=\"{Options.GraphBgColor}\"]";
            string nodeHeader = $"node [shape=none margin=0 colorscheme=\"{Options.NodeColorScheme}\" fillcolor=\"{Options.NodeDefaultFillColor}\" fontcolor=\"{Options.NodeFontColor}\" fontname=\"{Options.FontName}\" fontsize={Options.NodeLabelFontSize}]";
            string edgeHeader = $"edge [colorscheme=\"{Options.EdgeColorScheme}\"]";
            sw.WriteLine(fileHeader);
            sw.WriteLine(nodeHeader);
            sw.WriteLine(edgeHeader);

            string cluster = @"
subgraph cluster_levels
{
	label = """"
	bgcolor = ""6""
	pencolor = ""6""

	node [fontsize=48 margin=.5]
	X1 [label=""X""]
	O1 [label=""O""]
	X2 [label=""X""]
	O2 [label=""O""]
	X3 [label=""X""]
	O3 [label=""O""]
	X4 [label=""X""]
	O4 [label=""O""]
	X5 [label=""X""]

	edge [penwidth=5 arrowsize=2 style=""filled"" colorscheme=""piyg11""]
	X1 -> O1 [color=""3""]
	O1 -> X2 [color=""8""]
	X2 -> O2 [color=""3""]
	O2 -> X3 [color=""8""]
	X3 -> O3 [color=""3""]
	O3 -> X4 [color=""8""]
	X4 -> O4 [color=""3""]
	O4 -> X5 [color=""8""]
}
";
            sw.WriteLine(cluster);

        }

        protected override void WriteDotFileNodes(Node root, Dictionary<int, List<Node>> treeLevels, StreamWriter sw)
        {
            // do nothing
            // we write the nodes while writing the edges
        }

        public override void WriteDotFileEdges(Node node, Dictionary<int, List<Node>> treeLevels, StreamWriter sw)
        {
            sw.WriteLine($"__________ -> _____X____ [style=\"dashed\"] ");
            sw.WriteLine($"__________ -> ________X_ [style=\"dashed\"] ");
            sw.WriteLine($"__________ -> invis1 [style=\"invis\"] ");
            sw.WriteLine($"invis1 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis2 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis3 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis4 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis5 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis6 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis7 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis8 [shape=\"none\" style=\"invis\"]");
            sw.WriteLine($"invis1 -> invis2 [style=\"invis\"]");
            sw.WriteLine($"invis2 -> invis3 [style=\"invis\"]");
            sw.WriteLine($"invis3 -> invis4 [style=\"invis\"]");
            sw.WriteLine($"invis4 -> invis5 [style=\"invis\"]");
            sw.WriteLine($"invis5 -> invis6 [style=\"invis\"]");
            sw.WriteLine($"invis6 -> invis7 [style=\"invis\"]");
            sw.WriteLine($"invis7 -> invis8 [style=\"invis\"]");

            foreach (Node n in treeLevels[TreeHelpers.LeafLevel])
            {
                int height = BestMoveSubtreeHeight(n);
                if (height < 5) continue;
                WriteNode(node, sw);
                WriteDotFileEdgesFromLeaves(n, sw);
            }
        }

        private void WriteDotFileEdgesFromLeaves(Node node, StreamWriter sw)
        {
            WriteNode(node, sw);
            if (!node.BestParents.Any() && node.Board.NextMoveNum > 1)
            {
                int invisParentNum = node.Board.NextMoveNum - 1;
                sw.WriteLine($"invis{invisParentNum} -> _{node.Board.KeyString()} [style=\"invis\"]");
                return;
            }
            foreach (Node n in node.BestParents)
            {
                sw.WriteLine($"_{n.Board.KeyString()} -> _{node.Board.KeyString()} [color=\"{Options.EdgeHighlightColor}\"] ");
                WriteDotFileEdgesFromLeaves(n, sw);
            }
        }

        private int BestMoveSubtreeHeight(Node node)
        {
            Node current = node;
            int height = 1;
            foreach (Node parent in current.BestParents)
            {
                int newHeight = BestMoveSubtreeHeight(parent) + 1;
                height = Math.Max(height, newHeight);
            }
            return height;
        }
    }
}
