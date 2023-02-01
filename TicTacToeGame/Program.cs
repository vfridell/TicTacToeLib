using System.Xml.Linq;
using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Program
    {
        static int LeafLevel = 99;
        static void Main(string[] args)
        {
            Board initialBoard = new Board();
            Node root = new(initialBoard);
            List<Node> leaves = new();
            Dictionary<int, List<Node>> treeLevels = new();
            GenerateFutureBoards(root, 0, treeLevels);
            int xwins = treeLevels[LeafLevel].Count(n => n.Board.GetResult() == GameResult.X_Win);
            int owins = treeLevels[LeafLevel].Count(n => n.Board.GetResult() == GameResult.O_Win);
            int draws = treeLevels[LeafLevel].Count(n => n.Board.GetResult() == GameResult.Draw);

            using (StreamWriter sw = new StreamWriter(File.Open("TicTacToe.dot", FileMode.Create, FileAccess.Write), System.Text.Encoding.ASCII))
            {
                sw.NewLine = "\n";
                WriteDotFileHeaders(sw);
                WriteDotFileNodes(treeLevels, sw);
                WriteNode(root, sw);
                WriteDotFileEdges(root, sw);
                sw.WriteLine("}");
            }
        }

        static void GenerateFutureBoards(Node node, int level, Dictionary<int, List<Node>> treeLevels)
        {
            IEnumerable<Move> moves = node.Board.GetAvailableMoves();
            if (!moves.Any())
            {
                if (!treeLevels.ContainsKey(LeafLevel)) treeLevels[LeafLevel] = new List<Node>();
                treeLevels[LeafLevel].Add(node);
                
                return;
            }
            foreach (Move move in moves)
            {
                var newBoard = node.Board.Clone();
                newBoard.TryMove(move);
                if (!treeLevels.ContainsKey(level)) treeLevels[level] = new List<Node>();
                Node? equivalentNode = BoardIsomorphComparer.GetFirstIsomorphDuplicate(newBoard, treeLevels[level]);
                if (equivalentNode != null)
                {
                    node.Futures.Add(equivalentNode);
                }
                else
                {
                    Node newNode = new Node(newBoard);
                    node.Futures.Add(newNode);
                    treeLevels[level].Add(newNode);
                    GenerateFutureBoards(newNode, level + 1, treeLevels);
                }
            }
        }

        static void WriteDotFileHeaders(StreamWriter sw)
        {
            string fileHeader = "strict digraph test\n{\ngraph[overlap = \"false\", splines = \"true\", rankdir=LR]";
            string nodeHeader = "node [style=\"filled\",shape=none,fillcolor=white,fontcolor=black,fontname=\"courier new\", fontsize=8]";
            sw.WriteLine(fileHeader);
            sw.WriteLine(nodeHeader);
        }

        static void WriteNode(Node n, StreamWriter sw)
        {
            sw.WriteLine($"_{n.Board.KeyString()} [label={n.Board.GetLabel()}]");
        }

        static void WriteDotFileNodes(Dictionary<int, List<Node>> treeLevels, StreamWriter sw)
        {
            for (int i = 0; i < 9; i++)
            {
                foreach (Node n in treeLevels[i])
                {
                    WriteNode(n, sw);
                }
            }
        }

        static void WriteDotFileEdges(Node node, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            foreach (Node n in node.Futures)
            {
                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} ");
                WriteDotFileEdges(n, sw);
            }
        }
    }
}