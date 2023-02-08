using System.Xml.Linq;
using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Program
    {
        //static ColorPicker colors = new("1", "2", "3","4","5","6","7","8","9");
        static ColorPicker colors = new("4","5","6","7","8","9");
        static int LeafLevel = 99;
        static void Main(string[] args)
        {
            Board initialBoard = new Board();
            Node root = new(initialBoard, BoardScorer.GetScore(initialBoard));
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
                WriteDotFileEdgesMultiColor(root, sw);
                //WriteDotFileEdgesBestMove(root, sw);
                //WriteDotFileEdgesScoreDiff(root, sw);
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
                    //Node newNode = new Node(newBoard); // no scores
                    Node newNode = new Node(newBoard, BoardScorer.GetScore(newBoard));
                    node.Futures.Add(newNode);
                    treeLevels[level].Add(newNode);
                    GenerateFutureBoards(newNode, level + 1, treeLevels);
                }
            }
        }

        static void WriteDotFileHeaders(StreamWriter sw)
        {
            string fileHeader = "strict digraph TicTacToe\n{\ngraph[label=\"Tic-Tac-Toe\" labelloc=t labeljust=c fontsize=128 overlap = \"false\", splines = \"true\", rankdir=TB, ranksep=\"5 equally\", colorscheme=\"oranges9\", bgcolor=2]";
            string nodeHeader = "node [style=\"filled\",shape=none,fillcolor=\"transparent\",fontcolor=black,fontname=\"courier new\", fontsize=8]";
            string edgeHeader = "edge [colorscheme=oranges9]";
            sw.WriteLine(fileHeader);
            sw.WriteLine(nodeHeader);
            sw.WriteLine(edgeHeader);
        }

        static void WriteNode(Node n, StreamWriter sw)
        {
            sw.WriteLine($"_{n.Board.KeyString()} [label={GetNodeLabel(n)}]");
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

        static void WriteDotFileEdgesScoreDiff(Node node, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            int score = node.Score ?? 0;

            foreach (Node n in node.Futures)
            {
                int nextScore = n.Score ?? 0;
                string color;
                if (Math.Abs(score) < 500 && Math.Abs(nextScore) >= 500) color = "3";
                else if (Math.Abs(score) >= 500 && Math.Abs(nextScore) <= 500) color = "3";
                else if (Math.Abs(score) < 500 && Math.Abs(nextScore) < 500) color = "6";
                else color = "3";

                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{color}\"] ");
                WriteDotFileEdgesScoreDiff(n, sw);
            }
        }

        static void WriteDotFileEdgesMultiColor(Node node, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            foreach (Node n in node.Futures)
            {
                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{colors.Next()}\"] ");
                WriteDotFileEdgesMultiColor(n, sw);
            }
        }

        static void WriteDotFileEdgesBestMove(Node node, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            int bestFutureScore = 0;
            if (node.Board.PieceToMove == 1) bestFutureScore = node.Futures.Max(n => n.Score ?? 0);
            else bestFutureScore = node.Futures.Min(n => n.Score ?? 0);
            foreach (Node n in node.Futures)
            {
                string color;
                if (n.Score == bestFutureScore) color = "9";
                else color = "3";

                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{color}\"] ");
                WriteDotFileEdgesBestMove(n, sw);
            }
        }

        static string GetNodeLabel(Node node)
        {
            string[] pieces = node.Board.GetPiecesByPosition();
            string color = "";
            var result = node.Board.GetResult();
            switch (result)
            {
                case GameResult.Draw:
                    color = "bgcolor=\"aqua\"";
                    break;
                case GameResult.O_Win:
                    color = "bgcolor=\"yellowgreen\"";
                    break;
                case GameResult.X_Win:
                    color = "bgcolor=\"tomato\"";
                    break;
                default:
                    color = "";
                    break;

            };
            string scorePart = "";
            if(node.Score.HasValue) scorePart = $"<tr><td border=\"0\" COLSPAN=\"3\">{node.Score}</td></tr>";
            return $"<<table {color} border=\"0\" cellborder=\"1\" cellspacing=\"0\"><tr><td port=\"p8\">{pieces[8]}</td><td port=\"p7\">{pieces[7]}</td><td port=\"p6\">{pieces[6]}</td></tr><tr><td port=\"p5\">{pieces[5]}</td><td port=\"p4\">{pieces[4]}</td><td port=\"p3\">{pieces[3]}</td></tr><tr><td port=\"p2\">{pieces[2]}</td><td port=\"p1\">{pieces[1]}</td><td port=\"p0\">{pieces[0]}</td></tr>{scorePart}</table>>";
        }
    }
}