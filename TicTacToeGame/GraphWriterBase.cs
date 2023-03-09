using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeGame
{
    public abstract class GraphWriterBase
    {
        public GraphOptions Options { get; set; }

        public GraphWriterBase(GraphOptions options)
        {
            Options = options;
        }

        public abstract void WriteDotFileEdges(Node node, Dictionary<int, List<Node>> treeLevels, StreamWriter sw);

        public virtual void WriteDotFileHeaders(StreamWriter sw)
        {
            string fileHeader = $"strict digraph TicTacToe\n{{\ngraph[label=\"{Options.GraphLabel}\" labelloc=t labeljust=c fontsize={Options.GraphLabelFontSize} overlap = \"false\", splines = \"true\", rankdir=TB, ranksep=\"{Options.RankSeparation} equally\", colorscheme=\"{Options.ColorScheme}\", bgcolor=\"{Options.GraphBgColor}\"]";
            string nodeHeader = $"node [style=\"filled\",shape=none,fillcolor=\"{Options.NodeDefaultFillColor}\",fontcolor=\"{Options.NodeFontColor}\",fontname=\"{Options.FontName}\", fontsize={Options.NodeLabelFontSize}]";
            string edgeHeader = $"edge [colorscheme=\"{Options.ColorScheme}\"]";
            sw.WriteLine(fileHeader);
            sw.WriteLine(nodeHeader);
            sw.WriteLine(edgeHeader);
        }

        public virtual void WriteDotFile(string filename, Node root, Dictionary<int, List<Node>> treeLevels)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), System.Text.Encoding.ASCII))
            {
                sw.NewLine = "\n";
                WriteDotFileHeaders(sw);
                WriteDotFileNodes(root, treeLevels, sw);
                WriteDotFileEdges(root, treeLevels, sw);
                sw.WriteLine("}");
            }
        }

        protected virtual void WriteDotFileNodes(Node root, Dictionary<int, List<Node>> treeLevels, StreamWriter sw)
        {
            WriteNode(root, sw);
            for (int i = 0; i < 9; i++)
            {
                foreach (Node n in treeLevels[i])
                {
                    WriteNode(n, sw);
                }
            }
        }

        protected virtual void WriteNode(Node n, StreamWriter sw)
        {
            sw.WriteLine($"_{n.Board.KeyString()} [label={GetNodeLabel(n)}]");
        }

        protected virtual string GetNodeLabel(Node node)
        {
            string[] pieces = node.Board.GetPiecesByPosition();
            string color = "";
            var result = node.Board.GetResult();
            switch (result)
            {
                case GameResult.Draw:
                    color = $"bgcolor=\"{Options.DrawNodeColor}\"";
                    break;
                case GameResult.O_Win:
                    color = $"bgcolor=\"{Options.OWinNodeColor}\"";
                    break;
                case GameResult.X_Win:
                    color = $"bgcolor=\"{Options.XWinNodeColor}\"";
                    break;
                default:
                    color = "";
                    break;
            };
            if (string.IsNullOrEmpty(color) && node.Score >= 400) color = $"bgcolor=\"{Options.XAlmostWinColor}\"";
            if (string.IsNullOrEmpty(color) && node.Score <= -400) color = $"bgcolor=\"{Options.OAlmostWinColor}\"";
            if (string.IsNullOrEmpty(color) && node.Score >= 14) color = $"bgcolor=\"{Options.XAdvantageColor}\"";
            if (string.IsNullOrEmpty(color) && node.Score <= -14) color = $"bgcolor=\"{Options.OAdvantageColor}\"";
            string scorePart = "";
            if (node.Score.HasValue && Options.ShowScore) scorePart = $"<tr><td border=\"0\" COLSPAN=\"3\">{node.Score}</td></tr>";
            return $"<<table {color} border=\"0\" cellborder=\"1\" cellspacing=\"0\"><tr><td port=\"p8\" sides=\"RB\">{pieces[8]}</td><td port=\"p7\" sides=\"RL\">{pieces[7]}</td><td port=\"p6\" sides=\"LB\">{pieces[6]}</td></tr><tr><td port=\"p5\" sides=\"TB\">{pieces[5]}</td><td port=\"p4\" border=\"0\">{pieces[4]}</td><td port=\"p3\" sides=\"TB\">{pieces[3]}</td></tr><tr><td port=\"p2\" sides=\"TR\">{pieces[2]}</td><td port=\"p1\" sides=\"RL\">{pieces[1]}</td><td port=\"p0\" sides=\"LT\">{pieces[0]}</td></tr>{scorePart}</table>>";
        }
    }
}
