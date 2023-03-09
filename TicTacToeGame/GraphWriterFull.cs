using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeGame
{
    public class GraphWriterFull : GraphWriterBase
    {
        public GraphWriterFull(GraphOptions options) : base(options) { }
        private ColorPicker _colors = new("4","5","6","7","8","9");

        public override void WriteDotFileEdges(Node node, Dictionary<int, List<Node>> _, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            foreach (Node n in node.Futures)
            {
                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{_colors.Next()}\"] ");
                WriteDotFileEdges(n, _, sw);
            }
        }
    }
}
