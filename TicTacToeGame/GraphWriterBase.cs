using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            string fileHeader = $"strict digraph TicTacToe\n{{\ngraph[label=\"{Options.GraphLabel}\" pad=1 labelloc=t labeljust=c fontsize={Options.GraphLabelFontSize} overlap = \"false\" splines = \"true\" rankdir=TB ranksep=\"{Options.RankSeparation} equally\" colorscheme=\"{Options.GraphColorScheme}\" bgcolor=\"{Options.GraphBgColor}\"]";
            string nodeHeader = $"node [shape=none margin=0 colorscheme=\"{Options.NodeColorScheme}\" fillcolor=\"{Options.NodeDefaultFillColor}\" fontcolor=\"{Options.NodeFontColor}\" fontname=\"{Options.FontName}\" fontsize={Options.NodeLabelFontSize}]";
            string edgeHeader = $"edge [colorscheme=\"{Options.EdgeColorScheme}\"]";
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
            string color = "";
            string colorScheme = "";
            var result = n.Board.GetResult();
            (colorScheme, color) = (Node: n, Result: result) switch
            {
                { Result: GameResult.Draw} => ($"colorscheme=\"{Options.NodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.TieNodeColor}\""),
                { Result: GameResult.X_Win } => ($"colorscheme=\"{Options.XNodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.XWinNodeColor}\""),
                { Result: GameResult.O_Win } => ($"colorscheme=\"{Options.ONodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.OWinNodeColor}\""),
                { Node.Score: >= 400 } => ($"colorscheme=\"{Options.XNodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.XAlmostWinColor}\""),
                { Node.Score: >= 14 } => ($"colorscheme=\"{Options.XNodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.XAdvantageColor}\""),
                { Node.Score: <= -400 } => ($"colorscheme=\"{Options.ONodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.OAlmostWinColor}\""),
                { Node.Score: <= -14 } => ($"colorscheme=\"{Options.ONodeColorScheme}\"", $"color=\"{Options.NodeFontColor}\" bgcolor=\"{Options.OAdvantageColor}\""),
                _ => ("", $"color=\"{Options.NodeFontColor}\""),
            };

            sw.WriteLine($"_{n.Board.KeyString()} [{colorScheme} label={GetNodeLabel(n, color)}]");
        }

        protected virtual string GetNodeLabel(Node node, string color)
        {
            string[] pieces = node.Board.GetPiecesByPosition();
            string scorePart = "";
            if (node.Score.HasValue && Options.ShowScore) scorePart = $"<tr><td border=\"0\" COLSPAN=\"3\"><font point-size=\"{Options.NodeLabelFontSize / 2}\">{node.Score}</font></td></tr>";
            return $"<<table {color} border=\"0\" cellborder=\"1\" cellspacing=\"0\"><tr><td port=\"p8\" sides=\"RB\">{pieces[8]}</td><td port=\"p7\" sides=\"RL\">{pieces[7]}</td><td port=\"p6\" sides=\"LB\">{pieces[6]}</td></tr><tr><td port=\"p5\" sides=\"TB\">{pieces[5]}</td><td port=\"p4\" >{pieces[4]}</td><td port=\"p3\" sides=\"TB\">{pieces[3]}</td></tr><tr><td port=\"p2\" sides=\"TR\">{pieces[2]}</td><td port=\"p1\" sides=\"RL\">{pieces[1]}</td><td port=\"p0\" sides=\"LT\">{pieces[0]}</td></tr>{scorePart}</table>>";
        }
    }
}
