using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeGame
{
    public class GraphWriterBestMovesOnlyForest : GraphWriterBase
    {
        public GraphWriterBestMovesOnlyForest(GraphOptions options) : base(options) { }

        public override void WriteDotFileEdges(Node node, Dictionary<int, List<Node>> _, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;

            // force a link that makes the tree look more rooted 
            if (node.Board.NextMoveNum == 0)
            {
                sw.WriteLine($"__________ -> _____X____ [style=\"dashed\"] ");
                sw.WriteLine($"__________ -> ________X_ [style=\"dashed\"] ");
            }

            foreach (Node n in node.Futures)
            {
                if (n == node.BestFuture)
                {
                    sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{Options.EdgeHighlightColor}\"] ");
                }
                else if (node.Board.NextMoveNum > 0 && node.Board.NextMoveNum < 2)
                {
                    sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [style=\"dashed\"] ");
                }
                else if (!n.BestParents.Any())
                {
                    // add fake nodes to adjust rank
                    int invisParentNum = node.Board.NextMoveNum;
                    for (int i = 0; i < invisParentNum; i++)
                    {
                        sw.WriteLine($"invis{i}_{n.Board.KeyString()} [style=\"invis\"] ");
                        sw.WriteLine($"invis{i}_{n.Board.KeyString()} -> invis{i + 1}_{n.Board.KeyString()} [style=\"invis\"] ");
                    }
                    sw.WriteLine($"invis{invisParentNum}_{n.Board.KeyString()} [style=\"invis\"] ");
                    sw.WriteLine($"invis{invisParentNum}_{n.Board.KeyString()} -> _{n.Board.KeyString()} [style=\"invis\"] ");
                }
                WriteDotFileEdges(n, _, sw);
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
                string color = Options.EdgeHighlightColor;

                sw.WriteLine($"_{n.Board.KeyString()} -> _{node.Board.KeyString()} [color=\"{color}\"] ");
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
