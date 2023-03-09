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
