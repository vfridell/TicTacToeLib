using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeGame
{
    public class GraphWriterHighlightBestMove : GraphWriterBase
    {
        public GraphWriterHighlightBestMove(GraphOptions options) : base(options) { }

        public override void WriteDotFileEdges(Node node, Dictionary<int, List<Node>> _, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            foreach (Node n in node.Futures)
            {
                string color;
                if (n == node.BestFuture) color = Options.EdgeHighlightColor;
                else color = Options.EdgeMinimalColor; 

                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{color}\"] ");
                WriteDotFileEdges(n, _, sw);
            }
        }
    }
}
