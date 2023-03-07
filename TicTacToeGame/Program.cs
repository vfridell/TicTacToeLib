using System.Xml.Linq;
using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Program
    {
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

            NegamaxAllTheNodes(root, treeLevels);
            
            WriteDotFile("TicTacToe_multicolor.dot", 10, root, treeLevels, WriteDotFileEdgesMultiColor);
            WriteDotFile("TicTacToe_HighlightBestScore.dot", 10, root, treeLevels, WriteDotFileEdgesBestScore);
            WriteDotFile("TicTacToe_bestScoreForest.dot", 5, root, treeLevels, WriteDotFileEdgesBestScoreOnly);
            WriteDotFile("TicTacToe_HighlightBestMove.dot", 10, root, treeLevels, WriteDotFileEdgesBestMove);
            WriteDotFile("TicTacToe_bestMoveForest.dot", 5, root, treeLevels, WriteDotFileEdgesBestMoveOnly);

        }

        static void WriteDotFile(string filename, int rankSep, Node root, Dictionary<int, List<Node>> treeLevels, Action<Node, StreamWriter> edgeLogic)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), System.Text.Encoding.ASCII))
            {
                sw.NewLine = "\n";
                WriteDotFileHeaders(rankSep, sw);
                WriteDotFileNodes(root, treeLevels, sw);
                edgeLogic(root, sw);
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
                    if(!node.Futures.Contains(equivalentNode)) node.Futures.Add(equivalentNode);
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

        static void NegamaxAllTheNodes(Node root, Dictionary<int, List<Node>> treeLevels)
        {
            // there are 10 node levels, including the root node
            int player = 1;
            // iterative deepening limit
            int maxDepth = 9;
            int depth = 1;
            int val = 0;

            // look for the shallowest win by iterative deepening
            while (val < 100 && depth <= maxDepth)
            {
                val = NegamaxRoot(root, player, depth);
                depth++;
            }
            root.BestFuture!.BestParent = root;

            for (int l = 0; l < 9; l++)
            {
                player = -player;
                foreach (Node node in treeLevels[l])
                {
                    depth = 1;
                    val = 0;
                    // look for the shallowest win by iterative deepening
                    while (val < 100 && depth <= maxDepth)
                    {
                        val = NegamaxRoot(node, player, depth);
                        depth++;
                    }
                    if (node.BestFuture != null) node.BestFuture.BestParent = node;
                }
            }
        }

        static int NegamaxRoot(Node node, int player, int depth)
        {
            Node bestNode = null!;
            int value = int.MinValue;
            foreach (Node future in node.Futures)
            {
                int newValue = -Negamax(future, -player, depth - 1);
                if (newValue > value)
                {
                    bestNode = future;
                    value = newValue;
                }
            }
            node.BestFuture = bestNode;
            return value;
        }

        static int Negamax(Node node, int player, int depth)
        {
            if (depth == 0 || node.Futures.Count == 0)
            {
                if (!node.Score.HasValue) node.Score = BoardScorer.GetScore(node.Board);
                return node.Score.Value * player;
            }
            int value = int.MinValue;
            foreach (Node future in node.Futures)
            {
                value = Math.Max(value, -Negamax(future, -player, depth - 1));
            }
            return value;
        }

        static void WriteDotFileHeaders(int rankSep, StreamWriter sw)
        {
            string fileHeader = $"strict digraph TicTacToe\n{{\ngraph[label=\"Tic-Tac-Toe\" labelloc=t labeljust=c fontsize=128 overlap = \"false\", splines = \"true\", rankdir=TB, ranksep=\"{rankSep} equally\", colorscheme=\"oranges9\", bgcolor=2]";
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

        static void WriteDotFileNodes(Node root, Dictionary<int, List<Node>> treeLevels, StreamWriter sw)
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
            foreach (Node n in node.Futures)
            {
                string color;
                if (n == node.BestFuture) color = "9";
                else color = "3";

                sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{color}\"] ");
                WriteDotFileEdgesBestMove(n, sw);
            }
        }

        static void WriteDotFileEdgesBestMoveOnly(Node node, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            string color = "9";

            // force a link that makes the tree look cool
            if (node.Board.State.Count(m => m != null) == 0)
            {
                sw.WriteLine($"__________ -> _____X____ [color=\"{color}\"] ");
            }

            foreach (Node n in node.Futures)
            {
                if (n == node.BestFuture)
                {
                    sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{color}\"] ");
                }
                else if(n.BestParent == null)
                {
                    // TODO add fake nodes to adjust rank
                }
                WriteDotFileEdgesBestMoveOnly(n, sw);
            }

        }

        static void WriteDotFileEdgesBestScore(Node node, StreamWriter sw)
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
                WriteDotFileEdgesBestScore(n, sw);
            }
        }

        static void WriteDotFileEdgesBestScoreOnly(Node node, StreamWriter sw)
        {
            if (!node.Futures.Any()) return;
            int bestFutureScore = 0;
            if (node.Board.PieceToMove == 1) bestFutureScore = node.Futures.Max(n => n.Score ?? 0);
            else bestFutureScore = node.Futures.Min(n => n.Score ?? 0);
            foreach (Node n in node.Futures)
            {
                string color = "9";
                if (n.Score == bestFutureScore)
                {
                    sw.WriteLine($"_{node.Board.KeyString()} -> _{n.Board.KeyString()} [color=\"{color}\"] ");
                }
                WriteDotFileEdgesBestScoreOnly(n, sw);
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
                    color = "bgcolor=\"darkturquoise\"";
                    break;
                case GameResult.O_Win:
                    color = "bgcolor=\"limegreen\"";
                    break;
                case GameResult.X_Win:
                    color = "bgcolor=\"orangered\"";
                    break;
                default:
                    color = "";
                    break;
            };
            if(string.IsNullOrEmpty(color) && node.Score >= 400) color = "bgcolor=\"tomato\"";
            if(string.IsNullOrEmpty(color) && node.Score <= -400) color = "bgcolor=\"springgreen\"";
            if(string.IsNullOrEmpty(color) && node.Score >= 14) color = "bgcolor=\"yellow\"";
            if(string.IsNullOrEmpty(color) && node.Score <= -14) color = "bgcolor=\"yellow\"";
            string scorePart = "";
            if(node.Score.HasValue) scorePart = $"<tr><td border=\"0\" COLSPAN=\"3\">{node.Score}</td></tr>";
            return $"<<table {color} border=\"0\" cellborder=\"1\" cellspacing=\"0\"><tr><td port=\"p8\" sides=\"RB\">{pieces[8]}</td><td port=\"p7\" sides=\"RL\">{pieces[7]}</td><td port=\"p6\" sides=\"LB\">{pieces[6]}</td></tr><tr><td port=\"p5\" sides=\"TB\">{pieces[5]}</td><td port=\"p4\">{pieces[4]}</td><td port=\"p3\" sides=\"TB\">{pieces[3]}</td></tr><tr><td port=\"p2\" sides=\"TR\">{pieces[2]}</td><td port=\"p1\" sides=\"RL\">{pieces[1]}</td><td port=\"p0\" sides=\"LT\">{pieces[0]}</td></tr>{scorePart}</table>>";
        }
    }
}