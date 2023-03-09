using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public class TreeHelpers
    {
        public static readonly int LeafLevel = 99;

        public static void GenerateFutureBoards(Node node, int level, Dictionary<int, List<Node>> treeLevels)
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
                    if (!node.Futures.Contains(equivalentNode)) node.Futures.Add(equivalentNode);
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

        public static void NegamaxAllTheNodes(Node root, Dictionary<int, List<Node>> treeLevels)
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
            root.BestFuture!.BestParents.Add(root);

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
                    if (node.BestFuture != null) node.BestFuture.BestParents.Add(node);
                }
            }
        }

        public static int NegamaxRoot(Node node, int player, int depth)
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

        private static int Negamax(Node node, int player, int depth)
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
    }
}
